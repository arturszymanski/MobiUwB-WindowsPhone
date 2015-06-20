#region

using System;

#endregion

namespace SharedCode.DataManagment.DataAccess
{
    public interface IReadData
    {
        void Read<T>(out T obj, String key) where
            T : IRestolable<T>, new();
    }
}
