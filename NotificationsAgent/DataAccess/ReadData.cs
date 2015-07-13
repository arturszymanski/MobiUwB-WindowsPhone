#region

using System;
using System.IO.IsolatedStorage;
using SharedCode.DataManagment;
using SharedCode.DataManagment.DataAccess;

#endregion

namespace NotificationsAgent.DataAccess
{
    public class ReadData : IReadData
    {
        public void Read<T>(out T obj, String key) where 
            T : IRestolable<T>, new()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains(key))
            {
                 obj = (T)settings[key];
            }
            else
            {
                obj = new T().GetDefaults();
            }
        }
    }
}
