using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Parsers.Json.Model
{
    public class Feed
    {
        private String _title;
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }


        private String _content;
        public String Content
        {
            get { return _content; }
            set { _content = value; }
        }


        private DateTime _dateTime;
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }


        public Feed(String title, String content, DateTime dateTime)
        {
            this._title = title;
            this._content = content;
            this._dateTime = dateTime;
        }
    }
}
