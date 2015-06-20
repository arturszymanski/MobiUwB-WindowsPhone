#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.Properties
{
    public class Websites
    {
        [XmlElement("DefaultWebsite")]
        public Website DefaultWebsite { get; set; }

        [XmlElement("Website")]
        public List<Website> WebsiteList { get; set; }
    }
}
