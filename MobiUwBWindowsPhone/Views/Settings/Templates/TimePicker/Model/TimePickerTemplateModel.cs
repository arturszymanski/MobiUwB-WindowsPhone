#region

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Controls;

#endregion

namespace MobiUwB.Views.Settings.Templates.TimePicker.Model
{
    public delegate bool ValidateEvent(DateTime dt);
    [DataContract]
    public class TimePickerTemplateModel : TemplateModel, INotifyPropertyChanged
    {

        public event ValidateEvent Validate;
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _value;
        [DataMember]
        public DateTime Value
        {
            get { return _value; }
            set
            {
                bool isValid = true;
                if (Validate != null)
                {
                    isValid = Validate(value);
                }
                if (isValid)
                {
                    _value = value;
                }
                PropChanged("Value");
            }
        }

        private void PropChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
