#region

using System;
using System.Windows;
using MobiUwB.Views.Settings.Templates.CheckBoxItem.Model;
using MobiUwB.Views.Settings.Templates.ListPicker.Model;
using MobiUwB.Views.Settings.Templates.SwitchItem.Model;
using MobiUwB.Views.Settings.Templates.TimePicker.Model;

#endregion

namespace MobiUwB.Views.Settings.Templates.Selector
{
    public class SettingsTemplateSelector : TemplateSelector
    {
        private DataTemplate _listPickerTemplate;
        public DataTemplate ListPickerTemplate
        {
            get { return _listPickerTemplate; }
            set { _listPickerTemplate = value; }
        }

        private DataTemplate _switchTemplate;
        public DataTemplate SwitchTemplate
        {
            get { return _switchTemplate; }
            set { _switchTemplate = value; }
        }

        private DataTemplate _checkBoxTemplate;
        public DataTemplate CheckBoxTemplate
        {
            get { return _checkBoxTemplate; }
            set { _checkBoxTemplate = value; }
        }

        private DataTemplate _timePickerTemplate;

        public DataTemplate TimePickerTemplate
        {
            get { return _timePickerTemplate; }
            set { _timePickerTemplate = value; }
        }



        public override DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            if (item is SwitchTemplateModel)
            {
                return _switchTemplate;
            }
            if (item is CheckBoxTemplateModel)
            {
                return _checkBoxTemplate;
            }
            if (item is TimePickerTemplateModel)
            {
                return _timePickerTemplate;
            }
            if (item is IListPickerTemplateModel)
            {
                return _listPickerTemplate;
            }
            throw new ArgumentException("Unsuported Template");
        }
    }
}
