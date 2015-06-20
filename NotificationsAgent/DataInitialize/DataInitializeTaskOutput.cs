#region

using System;
using System.Collections.Generic;
using System.IO;
using SharedCode.Parsers.Models.ConfigurationXML;
using SharedCode.Parsers.Models.Properties;
using SharedCode.Tasks.Models;
using SharedCode.VersionControl.Models;

#endregion

namespace NotificationsAgent.DataInitialize
{
    public class DataInitializeTaskOutput : TaskOutput
    {
        public Stream propertiesFile;
        public Stream configurationFile;
        public Properties propertiesXmlResult;
        public VersioningResult versioningResult;
        public Configuration configXmlResult;

        public bool isNotificationActive;
        public bool isTimeRangeActive;

        public DateTime timeRangeFrom;
        public DateTime timeRangeTo;

        public int intervalIndex;
        public long interval;

        public Dictionary<String, Boolean> categories;

        public DataInitializeTaskOutput()
        {
            categories = new Dictionary<String, Boolean>();
        }
    }
}
