#region

using System;

#endregion

namespace SharedCode.VersionControl.Models
{
    public class VersioningRequest
    {
        private readonly String _internetFilePath;
        public String GetInternetFile()
        {
            return _internetFilePath;
        }

        private readonly String _destinationFileName;
        public String GetDestinationFileName()
        {
            return _destinationFileName;
        }

        public VersioningRequest(String internetFilePath, String destinationFileName)
        {
            _internetFilePath = internetFilePath;
            _destinationFileName = destinationFileName;
        }
    }
}
