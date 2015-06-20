#region

using SharedCode;
using SharedCode.Parsers.Models.ConfigurationXML;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace MobiUwB.StartupConfig.Worker
{
    public class StartupConfigurationResult : Result
    {
        public Properties Properties;
        public Configuration Configuration;
    }
}
