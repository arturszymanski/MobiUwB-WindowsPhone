using SharedCode.Tasks;
using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedCode.Utilities;
using SharedCode.Parsers.Models.ConfigurationXML;

namespace NotificationsAgent.DataInitialize.Tasks.CategoriesFinder
{
    public class CategoriesFinderTask : ITask<DataInitializeTaskOutput>
    {
        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            using (Mutex mutex = new Mutex(true, Variables.SettingsValuesMutexName))
            {
                mutex.WaitOne();
                try
                {
                    IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
                    if (isolatedStorageFile.FileExists(Variables.SettingsValuesFileName))
                    {
                        using (IsolatedStorageFileStream isolatedStorageFileStream = new
                            IsolatedStorageFileStream(
                            Variables.SettingsValuesFileName,
                            FileMode.Open,
                            isolatedStorageFile))
                        {
                            DataContractJsonSerializer dataContractJsonSerializer =
                                new DataContractJsonSerializer(
                                    typeof(SettingsCategoriesValuesWrapper));

                            SettingsCategoriesValuesWrapper settingsCategoriesValuesWrapper = 
                                (SettingsCategoriesValuesWrapper) dataContractJsonSerializer.ReadObject(
                                    isolatedStorageFileStream);
                            output.allValues = settingsCategoriesValuesWrapper.SettingsCategoriesValues;

                            FillValuesFromDictionary(output);
                        }
                    }
                    else
                    {
                        FillValuesFromConfigXml(output);
                    }
                }
                catch (Exception e)
                {
                    output.addError(e.Message);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private void FillValuesFromDictionary(DataInitializeTaskOutput output)
        {
        }

        private void FillValuesFromConfigXml(DataInitializeTaskOutput output)
        {
            //TODO id ...
            Unit unit = output.configXmlResult.GetUnitById("Id z dupy wzięte");
            output.isNotificationActive = IsNotificationsActive(unit);

            output.isTimeRangeActive = 
            //output.interval
            //output.intervalIndex
            //output.timeRangeFrom
            //output.timeRangeTo
            //output.categories
        }

        private Boolean IsNotificationsActive(Unit unit)
        {
            foreach (Section section in unit.Sections.SectionsList)
            {
                if (section.SectionNotifications)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
