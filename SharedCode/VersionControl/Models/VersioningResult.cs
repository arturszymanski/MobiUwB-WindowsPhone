#region



#endregion

#region

using System;

#endregion

namespace SharedCode.VersionControl.Models
{
    public class VersioningResult : Result
    {
        private VersioningRequest _versioningRequest;
        public VersioningRequest GetVersioningRequest()
        {
            return _versioningRequest;
        }
        public void SetVersioningRequest(
                VersioningRequest versioningRequest)
        {
            _versioningRequest = versioningRequest;
        }

        private String _fileContent;
        public String GetFileContent()
        {
            return _fileContent;
        }
        public void SetFileContent(String fileContent)
        {
            _fileContent = fileContent;
        }
    }
}
