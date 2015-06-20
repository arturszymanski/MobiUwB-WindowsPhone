#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.ConfigurationXML
{
    public class Sections
    {
        private List<Section> _sectionsLIst;
        [XmlElement("sekcja")]
        public List<Section> SectionsList
        {
            get { return _sectionsLIst; }
            set { _sectionsLIst = value; }
        }

        private List<StaticSection> _staticSectionsList;

        public List<StaticSection> StaticSectionList
        {
            get { return _staticSectionsList; }
            set { _staticSectionsList = value; }
        }
        
    }
}
