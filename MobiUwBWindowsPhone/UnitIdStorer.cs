using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MobiUwB.Utilities;
using SharedCode.Utilities;

namespace MobiUwB
{
    public class UnitIdStorer : BackgroundWorker
    {
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            String unitId = (String)e.Argument;
            StoreUnitId(unitId);
        }

        private void StoreUnitId(String unitId)
        {
            using (Mutex mutex = new Mutex(true, Variables.UnitIdMutexName))
            {
                mutex.WaitOne();
                try
                {
                    SerializeUnitIdWrapper(unitId);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private void SerializeUnitIdWrapper(String unitId)
        {
            UnitIdWrapper unitIdWrapper = new UnitIdWrapper();
            unitIdWrapper.UnitId = unitId;

            IsolatedStorageFile isolatedStorageFile =
                IsolatedStorageFile.GetUserStoreForApplication();

            using (IsolatedStorageFileStream isolatedStorageFileStream =
                new IsolatedStorageFileStream(
                    Variables.UnitIdFileName,
                    FileMode.OpenOrCreate,
                    isolatedStorageFile))
            {
                DataContractJsonSerializer dataContractJsonSerializer =
                    new DataContractJsonSerializer(
                        typeof(UnitIdWrapper));

                dataContractJsonSerializer.WriteObject(
                    isolatedStorageFileStream,
                    unitIdWrapper);
            }
        }
    }
}
