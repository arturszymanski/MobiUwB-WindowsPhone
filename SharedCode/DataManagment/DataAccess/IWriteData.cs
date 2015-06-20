#region

using System;

#endregion

namespace SharedCode.DataManagment.DataAccess
{
    public interface IWriteData
    {
        void Write<T>(T obj, String key) where
            T : IRestolable<T>, new();
    }
}
