#region

using System;
using SharedCode.DataManagment.DataAccess;

#endregion

namespace SharedCode.DataManagment
{
    public class DataManager
    {
        private readonly IReadData _readData;
        private readonly IWriteData _writeData;

        public DataManager(IReadData readData, IWriteData writeData)
        {
            _readData = readData;
            _writeData = writeData;
        }

        public T RestoreData<T>(String key) where
            T : IRestolable<T>, new()
        {
            T obj;
            _readData.Read(out obj, key);
            return obj;
        }

        public void StoreData<T>(T templateModel, String key) where
            T : IRestolable<T>, new()
        {
            _writeData.Write(templateModel, key);
        }
    }
}
