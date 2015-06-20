#region

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace MobiUwB.Views.Settings.Templates.TimePicker.Model
{
    [DataContract]
    public class TimePickerTemplateModel : TemplateModel, INotifyPropertyChanged
    {
        private DateTime _value;
        [DataMember]
        public DateTime Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void PropChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
