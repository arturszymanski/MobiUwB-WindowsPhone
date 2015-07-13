using SharedCode.Tasks;
using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsAgent.DataInitialize.Tasks.ConfigurationXml
{
    public class ConfigurationXmlTask : ITask<DataInitializeTaskOutput>
    {
        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            try
            {
                ScheduledAgent.XmlParser.Deserialize(
                    ScheduledAgent.ConfigurationFileName,
                    out output.configXmlResult);
            }
            catch (Exception e)
            {
                output.addError(e.Message);
            }
        }
    }
}
