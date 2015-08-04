using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.IO
{
    public class DownloadResult : IOResult
    {
        private DownloadInfo _information;

        public DownloadInfo Information
        {
            get { return _information; }
            private set { _information = value; }
        }

        public void AddException(DownloadInfo information, Exception exception = null)
        {
            if (exception != null)
            {
                AddException(exception);
            }
            Information = information;
        }
    }
}
