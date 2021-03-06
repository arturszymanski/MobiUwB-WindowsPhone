﻿using SharedCode.DataManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsAgent
{
    public class RestolableDateTime : IRestolable<RestolableDateTime>
    {
        private DateTime _dateTime;
        public DateTime DateTime
        {
            get { return _dateTime; }
            private set { _dateTime = value; }
        }

        public RestolableDateTime()
        {
            _dateTime = DateTime.MinValue;
        }

        public RestolableDateTime(DateTime dateTime)
        {
            this._dateTime = dateTime;
        }

        public RestolableDateTime GetDefaults()
        {
            return new RestolableDateTime();
        }
    }
}
