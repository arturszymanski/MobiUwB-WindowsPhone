#region

using System;
using System.IO.IsolatedStorage;
using SharedCode.DataManagment;
using SharedCode.DataManagment.DataAccess;

#endregion

namespace MobiUwB.DataAccess
{
    public class WriteData : IWriteData
    {
        public void Write<T>(T obj, String key) where
            T : IRestolable<T>, new()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (!settings.Contains(key))
            {
                settings.Add(key, obj);
            }
            else
            {
                settings[key] = obj;
            }
            settings.Save();
        }
    }
}
