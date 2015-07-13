using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsAgent.DataInitialize.Tasks.VersionController
{
    public class VersionControllerTaskInput : TaskInput
    {
        private String _configurationFileName;
        public String ConfigurationFileName
        {
            get { return _configurationFileName; }
            set { _configurationFileName = value; }
        }

        public VersionControllerTaskInput(String configurationFileName)
        {
            this._configurationFileName = configurationFileName;
        }
    }
}
