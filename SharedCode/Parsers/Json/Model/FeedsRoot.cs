using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Parsers.Json.Model
{
    [DataContract]
    public class FeedsRoot
    {
        private List<Feed> _feeds;
        [DataMember]
        public List<Feed> Feeds
        {
            get { return _feeds; }
            set { _feeds = value; }
        }

        public FeedsRoot()
        {
            _feeds = new List<Feed>();
        }
    }
}
