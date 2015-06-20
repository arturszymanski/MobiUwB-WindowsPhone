#region

using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.ConfigurationXML
{
    public class Map
    {
        private Coordinates _coordinates;
        [XmlElement("wspolrzedne")]
        public Coordinates Coordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }
    }
}
