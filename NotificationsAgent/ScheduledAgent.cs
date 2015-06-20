#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using NotificationsAgent.DataInitialize;
using SharedCode.Tasks;
using SharedCode.VersionControl;
using SharedCode.VersionControl.Models;
using SharedCode.Parsers.Models.ConfigurationXML;

#endregion

namespace NotificationsAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            bool succeeded = InitializeData();
            processNotifications(succeeded);

            NotifyComplete();
        }

        private ShellToast CreateShellToast(String title, String content)
        {
            ShellToast toast = new ShellToast();
            toast.Title = title;
            toast.Content = content;
            return toast;
        }

        private bool active;

        private DataInitializeTaskOutput dataInitializeTaskOutput;
        private long lastCheckTime;
        private long currentTime;

        private void processNotifications(bool succeeded)
        {
            if(dataInitializeTaskOutput.isNotificationActive &&
                    succeeded)
            {
                notificationsLoop();
            }
        }

        private void notificationsLoop()
        {
            long interval = dataInitializeTaskOutput.interval;
            DateTime from = dataInitializeTaskOutput.timeRangeFrom;
            DateTime to = dataInitializeTaskOutput.timeRangeTo;
            while(active)
            {
                notificationsLoopTick(interval, from, to);
            }
        }

        private void notificationsLoopTick(long interval, DateTime from, DateTime to)
        {
            currentTime = DateTime.Now.Millisecond;
            if(currentTime > lastCheckTime + interval)
            {
                if(dataInitializeTaskOutput.isTimeRangeActive)
                {
                    timeRangeNotificationsExecute(from, to);
                }
                else
                {
                    notificationsExecute();
                }
            }
            Thread.Sleep(1000);
        }

        private void notificationsExecute()
        {
            lastCheckTime = currentTime;
            runNotificationsPublisher();
        }

        private void timeRangeNotificationsExecute(DateTime from, DateTime to)
        {
            DateTime currentDate = new DateTime(currentTime);
            int currentHours = currentDate.Hour;
            int currentMinutes = currentDate.Minute;

            int fromHours = from.Hour;
            int fromMinutes = from.Minute;

            int toHours = to.Hour;
            int toMinutes = to.Minute;

            if(fromHours != toHours)
            {
                notificationsTimeRangeCheck(currentHours,
                                            fromHours,
                                            toHours);
            }
            else
            {
                notificationsTimeRangeCheck(currentMinutes,
                                            fromMinutes,
                                            toMinutes);
            }
        }

        private void notificationsTimeRangeCheck(int current, int from, int to)
        {
            if (from <= current &&
                    current <= to)
            {
                notificationsExecute();
            }
        }


        private void runNotificationsPublisher()
        {
            Unit unit = dataInitializeTaskOutput.configXmlResult.getCurrentUniversityUnit();
            int i = 0;
            foreach (String category in dataInitializeTaskOutput.categories)
            {
                if(category.getValue())
                {
                    Section section = unit.getSectionById(category.getKey());
                    List<Feed> newestFeeds = getNewElements(section, unit);
                    publishNotifications(newestFeeds,section,i);

                    ServicePreferencesManager servicePreferencesManager =
                            ServicePreferencesManager.getInstance(
                                    getBaseContext());

                    servicePreferencesManager.setLastKnownDate(
                            unit.Name,
                            category.getKey(),
                            DateTime.Now);
                }
                i++;
            }
        }

        private List<Feed> getNewElements(Section section, Unit unit)
        {
            List<Feed> notificationElements = new ArrayList<Feed>();

            SharedPreferences shareds = getSharedPreferences(
                    VersionControllerTask.SHARED_PREFERENCES_SERVICE_NAME,
                    Context.MODE_PRIVATE);

            VersionController versionController = new VersionController(shareds);
            String feedsFileUri = unit.getApiUrl() + section.id;

            Context baseContext = getBaseContext();

            String overwritePath = baseContext.getFilesDir().getPath();

            File overwriteFilePath = new File(overwritePath,section.id);


            VersioningRequest versioningRequest =
                    new VersioningRequest(feedsFileUri,
                                          overwriteFilePath.getAbsolutePath());

            VersioningResult versioningResult =
                    versionController.getNewestFile(versioningRequest);

            List<Feed> newestFeeds = new ArrayList<Feed>();
            if(versioningResult.getSucceeded())
            {
                String jsonContent = versioningResult.getFileContent();

                JsonParser jsonParser = new JsonParser();
                ServicePreferencesManager servicePreferencesManager =
                        ServicePreferencesManager.getInstance(getBaseContext());
                try
                {
                    notificationElements = jsonParser.parseFeedsJson(jsonContent);

                    Date lastKnownDate = servicePreferencesManager.getLastKnownDate(unit.Name,
                                                                                    section.id);


                    for(Feed feed : notificationElements)
                    {
                        if(feed.getDate().after(lastKnownDate))
                        {
                            newestFeeds.add(feed);
                        }
                    }

                }
                catch (JSONException e)
                {
                    Log.e("MobiUwB",e.getMessage());
                }
            }
            return newestFeeds;
        }

        private bool InitializeData()
        {
            TasksQueue<DataInitializeTaskOutput> tasksQueue =
                    new TasksQueue<DataInitializeTaskOutput>();


            dataInitializeTaskOutput = new DataInitializeTaskOutput();


            PropertiesXmlTask propertiesXmlTask = new PropertiesXmlTask();
            PropertiesXmlTaskInput propertiesXmlTaskInput = new PropertiesXmlTaskInput(
                "properties.xml",
                getBaseContext());
            tasksQueue.add(propertiesXmlTask, propertiesXmlTaskInput);


            VersionControllerTask versionControllerTask = new VersionControllerTask();
            VersionControllerTaskInput versionControllerTaskInput =
                new VersionControllerTaskInput(
                    "config.xml",
                    getBaseContext());
            tasksQueue.add(versionControllerTask, versionControllerTaskInput);


            ConfigurationXmlTask configurationXmlTask = new ConfigurationXmlTask();
            tasksQueue.add(configurationXmlTask, null);


            SettingsPreferenceManagerTask settingsPreferenceManagerTask =
                new SettingsPreferenceManagerTask();
            SettingsPreferenceManagerTaskInput settingsPreferenceManagerTaskInput =
                new SettingsPreferenceManagerTaskInput(
                    getBaseContext());
            tasksQueue.add(settingsPreferenceManagerTask, settingsPreferenceManagerTaskInput);


            tasksQueue.performAll(dataInitializeTaskOutput);

            return dataInitializeTaskOutput.isValid();
        }

        private void publishNotifications(List<Feed> newestFeeds,
                                          Section section,
                                          int notificationId)
        {
            int feedsAmount = newestFeeds.Count;
            if(feedsAmount > 0)
            {
                //TODO; String contentText = AppResources.

                ShellToast toast = CreateShellToast(section.title, contentText + feedsAmount);
                toast.Show();
            }
        }

    }
}