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
using SharedCode;
using SharedCode.Utilities;
using SharedCode.Parsers.Models.ConfigurationXML;

namespace NotificationsAgent.DataInitialize.Tasks.CategoriesFinder
{
    public class CategoriesFinderTask : ITask<DataInitializeTaskOutput>
    {
        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            using (Mutex mutex = new Mutex(true, output.CurrentUnitId))
            {
                mutex.WaitOne();
                try
                {
                    IsolatedStorageFile isolatedStorageFile = 
                        IsolatedStorageFile.GetUserStoreForApplication();
                    if (isolatedStorageFile.FileExists(output.CurrentUnitId))
                    {
                        using (IsolatedStorageFileStream isolatedStorageFileStream = new
                            IsolatedStorageFileStream(
                            output.CurrentUnitId,
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
                    output.AddException(e);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private void FillValuesFromDictionary(DataInitializeTaskOutput output)
        {
            Unit unit =
                output.configXmlResult.GetUnitById(
                    output.CurrentUnitId);

            output.isNotificationActive = IsNotificationsActive(output.allValues);
            output.isTimeRangeActive = GetBoolBy(Defaults.TimeRangeId,output.allValues);
            output.interval = GetInterval(output.allValues);
            output.intervalIndex = GetIntervalIndex(output.allValues);
            output.timeRangeFrom = GetTimeRange(output.allValues, Defaults.FromId);
            output.timeRangeTo = GetTimeRange(output.allValues, Defaults.ToId);
            output.categories = GetCategories(unit,output.allValues);
        }

        private bool IsNotificationsActive(Dictionary<string, object> allValues)
        {
            bool? notificationActive = allValues[Defaults.NotificationsActiveId] as bool?;
            if (notificationActive == true)
            {
                return true;
            }
            return false;
        }

        private void FillValuesFromConfigXml(DataInitializeTaskOutput output)
        {
            Unit unit =
                output.configXmlResult.GetUnitById(
                    output.CurrentUnitId);

            output.isNotificationActive = IsNotificationsActive(unit);
            output.isTimeRangeActive = Defaults.TimeRangeDefaultValue;
            output.interval = Defaults.Frequencies[Defaults.DefaultFrequencyIndex];
            output.intervalIndex = Defaults.DefaultFrequencyIndex;
            output.timeRangeFrom = Defaults.FromDefaultValue;
            output.timeRangeTo = Defaults.ToDefaultValue;
            output.categories = GetCategories(unit);
        }

        private Dictionary<string, bool> GetCategories(Unit unit)
        {
            Dictionary<string, bool> categories = new Dictionary<string, bool>();

            foreach (Section section in unit.Sections.SectionsList)
            {
                categories.Add(section.SectionId, section.SectionNotifications);
            }
            return categories;
        }

        private Dictionary<string, bool> GetCategories(Unit unit,
            Dictionary<string, object> allValues)
        {
            Dictionary<string, bool> categories = new Dictionary<string, bool>();

            foreach (Section section in unit.Sections.SectionsList)
            {
                string sectionId = section.SectionId;
                categories.Add(sectionId, GetBoolBy(sectionId,allValues));
            }
            return categories;
        }

        private DateTime GetTimeRange(Dictionary<string, object> allValues, string id)
        {
            DateTime? timeRangeFrom = allValues[id] as DateTime?;
            DateTime timeRange;
            if (timeRangeFrom == null)
            {
                timeRange = Defaults.FromDefaultValue;
            }
            else
            {
                timeRange = (DateTime)timeRangeFrom;
            }
            return timeRange;
        }

        private int GetIntervalIndex(Dictionary<string, object> allValues)
        {
            int? intervalIndex = allValues[Defaults.FrequencyId] as int?;
            int index;
            if (intervalIndex == null)
            {
                index = Defaults.DefaultFrequencyIndex;
            }
            else
            {
                index = (int)intervalIndex;
            }
            return index;
        }

        private long GetInterval(Dictionary<string, object> allValues)
        {
            return Defaults.Frequencies[GetIntervalIndex(allValues)];
        }

        private bool GetBoolBy(string key, Dictionary<string, object> allValues)
        {
            bool? value = allValues[key] as bool?;
            if (value == true)
            {
                return true;
            }
            return false;
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
