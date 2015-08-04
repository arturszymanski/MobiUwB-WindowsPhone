using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode
{
    public static class Defaults
    {

        public const String NotificationsActiveId = "notifications";
        public const String FrequencyId = "frequency";
        public const String TimeRangeId = "timeRange";
        public const String FromId = "from";
        public const String ToId = "to";
        public static readonly DateTime FromDefaultValue = DateTime.Now;
        public static readonly DateTime ToDefaultValue = DateTime.Now;
        public const int DefaultFrequencyIndex = 0;
        public const String currentUnitId = "currentUnitId";
        public const bool TimeRangeDefaultValue = true;

        public static readonly List<int> Frequencies = 
            new List<int>
            {
                60 * 1000, 
                10 * 60 * 1000, 
                3600 * 1000, 
                2 * 3600 * 1000, 
                6 * 3600 * 1000, 
                12 * 3600 * 1000, 
                24 * 3600 * 1000
            };
    }
}
