using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Parsers.Json.Model
{
    [DataContract]
    public class Feed
    {
        private String _title;
        [DataMember(Name="tytul")]
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }


        private String _content;
        [DataMember(Name="tresc")]
        public String Content
        {
            get { return _content; }
            set { _content = value; }
        }


        private DateTime _dateTime;
        [DataMember(Name = "data")]
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }
    }
}
