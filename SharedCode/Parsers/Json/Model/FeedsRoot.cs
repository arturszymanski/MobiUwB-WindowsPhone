using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Parsers.Json.Model
{
    [DataContract]
    public class FeedsRoot : IEnumerable<Feed>
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

        public IEnumerator<Feed> GetEnumerator()
        {
            return _feeds.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _feeds.GetEnumerator();
        }
    }
}
