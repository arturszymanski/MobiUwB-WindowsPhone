#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace MobiUwB.Views.Settings.Templates.ListPicker.Model
{
    [DataContract]
    public class ListPickerTemplateModel<TValue> : TemplateModel, INotifyPropertyChanged, IListPickerTemplateModel
    {
        private int _value;
        [DataMember]
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                PropChanged("Value");
            }
        }

        private List<ListPickerItem> _items;

        [DataMember]
        public List<ListPickerItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public ListPickerTemplateModel()
        {
            _items = new List<ListPickerItem>();
            Value = 0;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void PropChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        [DataContract]
        public class ListPickerItem
        {
            private String _title;

            [DataMember]
            public String Title
            {
                get { return _title; }
                set { _title = value; }
            }

            private TValue _value;
            [DataMember]
            public TValue Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                }
            }

            protected bool Equals(ListPickerItem other)
            {
                return Value.Equals(other.Value);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }
                if (obj.GetType() != GetType())
                {
                    return false;
                }
                return Equals((ListPickerItem) obj);
            }

            public override int GetHashCode()
            {
                return 7;
            }

            public ListPickerItem(String title, TValue value)
            {
                _title = title;
                _value = value;
            }

            public override string ToString()
            {
                return Title;
            }
        }
    }
}
