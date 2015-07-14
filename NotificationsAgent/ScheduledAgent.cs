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
using SharedCode.Parsers.Models;
using SharedCode.Parsers.Json.Model;
using NotificationsAgent.DataAccess;
using SharedCode.DataManagment;

using Section = SharedCode.Parsers.Models.ConfigurationXML.Section;
using Windows.Storage;
using System.Threading.Tasks;
using SharedCode.Parsers.Json;
using SharedCode.Parsers;
using NotificationsAgent.DataInitialize.Tasks.PropertiesXml;
using NotificationsAgent.DataInitialize.Tasks.VersionController;
using NotificationsAgent.DataInitialize.Tasks.ConfigurationXml;
using NotificationsAgent.DataInitialize.Tasks.CategoriesFinder;
#endregion

namespace NotificationsAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        public static readonly DataManager _dataManager;
        public static readonly WriteData WriteData;
        public static readonly ReadData ReadData;

        public static readonly XmlParser XmlParser;
        public static readonly IoManager IoManager;
        public static readonly String PropertiesFileName = "properties.xml";
        public static readonly String ConfigurationFileName = "configuration.xml";

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            WriteData = new WriteData();
            ReadData = new ReadData();
            _dataManager = new DataManager(ReadData, WriteData);
            IoManager = new IoManager();
            XmlParser = new XmlParser(IoManager);

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
            //TODO usunąć wykrzyknik
            if(!dataInitializeTaskOutput.isNotificationActive &&
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
            //TODO usunąć wykrzyknik
            while(!active)
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
            Unit unit = dataInitializeTaskOutput.configXmlResult.GetUnitById("Jakieś id ...");
            int i = 0;
            foreach (KeyValuePair<String, Boolean> category in dataInitializeTaskOutput.categories)
            {
                if(category.Value)
                {
                    Section section = unit.Sections.GetSectionById(category.Key);
                    List<Feed> newestFeeds = getNewElements(section, unit);
                    publishNotifications(newestFeeds,section,i);

                    RestolableDateTime RestolableDateTime = new RestolableDateTime(DateTime.Now);

                    _dataManager.StoreData(
                        RestolableDateTime, 
                        category.Key);
                }
                i++;
            }
        }

        private List<Feed> getNewElements(Section section, Unit unit)
        {
            VersionController versionController = 
                new VersionController(
                    IoManager, 
                    ReadData, 
                    WriteData);
            String feedsFileUri = unit.ApiUrlString + section.SectionId;

            String folder = ApplicationData.Current.LocalFolder.Path;
            String fullSavePath = Path.Combine(
                folder,
                section.SectionId);

            VersioningRequest versioningRequest =
                    new VersioningRequest(
                        feedsFileUri,
                        fullSavePath);

            Task<VersioningResult> versioningResultTask = 
                versionController.GetNewestFile(versioningRequest);

            VersioningResult versioningResult = versioningResultTask.Result;

            List<Feed> newestFeeds = new List<Feed>();
            if (versioningResult.Succeeded)
            {
                String jsonContent = versioningResult.GetFileContent();

                JsonParser jsonParser = new JsonParser();

                FeedsRoot notificationElements = 
                    jsonParser.ParseFeedsJson(jsonContent);

                RestolableDateTime lastKnownDate =
                    _dataManager.RestoreData<RestolableDateTime>(unit.Name);

                foreach (Feed feed in notificationElements)
                {
                    if (feed.DateTime > lastKnownDate.DateTime)
                    {
                        newestFeeds.Add(feed);
                    }
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
                "properties.xml");
            tasksQueue.add(propertiesXmlTask, propertiesXmlTaskInput);


            VersionControllerTask versionControllerTask = new VersionControllerTask();
            VersionControllerTaskInput versionControllerTaskInput =
                new VersionControllerTaskInput(
                    "config.xml");
            tasksQueue.add(versionControllerTask, versionControllerTaskInput);


            ConfigurationXmlTask configurationXmlTask = new ConfigurationXmlTask();
            tasksQueue.add(configurationXmlTask, null);


            CategoriesFinderTask settingsPreferenceManagerTask =
                new CategoriesFinderTask();
            tasksQueue.add(settingsPreferenceManagerTask, null);


            tasksQueue.performAll(dataInitializeTaskOutput);

            return dataInitializeTaskOutput.isValid();
        }

        private void publishNotifications(List<Feed> newestFeeds,
                                          Section section,
                                          int notificationId)
        {
            int feedsAmount = newestFeeds.Count;
            if (feedsAmount > 0)
            {
                ShellToast toast = CreateShellToast(section.SectionTitle,  
                    Resources.Resources.NotificationContentText + feedsAmount);
                toast.Show();
            }
        }

    }
}