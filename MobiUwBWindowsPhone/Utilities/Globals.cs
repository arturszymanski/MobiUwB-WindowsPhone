#region

using System;
using Windows.ApplicationModel.Store;
using MobiUwB.IO;

#endregion

namespace MobiUwB.Utilities
{
    public static class Globals
    {
        public static readonly IoManager IoManager = new IoManager();

        private static String _currentUnitId = "";
        public static string CurrentUnitId
        {
            get
            {
                lock (_currentUnitId)
                {
                    return _currentUnitId;
                }
            }
            set
            {
                lock (_currentUnitId)
                {
                    _currentUnitId = value;
                }
            }
        }
    }
}
