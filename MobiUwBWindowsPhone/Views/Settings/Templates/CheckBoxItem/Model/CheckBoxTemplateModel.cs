﻿#region

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace MobiUwB.Views.Settings.Templates.CheckBoxItem.Model
{
    [DataContract]
    public class CheckBoxTemplateModel : TemplateModel, INotifyPropertyChanged 
    {
        private Boolean _isChecked;
        [DataMember]
        public Boolean IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                PropChanged("IsChecked");
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
