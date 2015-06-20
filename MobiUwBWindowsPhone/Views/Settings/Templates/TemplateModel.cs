#region

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;
using MobiUwB.Resources;
using MobiUwB.StartupConfig;
using MobiUwB.Views.Settings.Templates.CheckBoxItem.Model;
using MobiUwB.Views.Settings.Templates.ListPicker.Model;
using MobiUwB.Views.Settings.Templates.SwitchItem.Model;
using MobiUwB.Views.Settings.Templates.TimePicker.Model;
using SharedCode.DataManagment;
using SharedCode.Parsers.Models.ConfigurationXML;

#endregion

namespace MobiUwB.Views.Settings.Templates
{
    [DataContract]
    [KnownType(typeof(TimePickerTemplateModel))]
    [KnownType(typeof(SwitchTemplateModel))]
    [KnownType(typeof(ListPickerTemplateModel<long>))]
    [KnownType(typeof(CheckBoxTemplateModel))]
    [KnownType(typeof(ListPickerTemplateModel<long>.ListPickerItem))]
    public class TemplateModel : IRestolable<TemplateModel>
    {
        private List<TemplateModel> _children;
        [DataMember]
        public List<TemplateModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        private String _text;
        [DataMember]
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public TemplateModel()
        {
            _children = new List<TemplateModel>();
        }

        public TemplateModel GetDefaults()
        {
            SwitchTemplateModel notifications = new SwitchTemplateModel();
            notifications.Text = AppResources.SettingsPageCategoryNotifications;
            notifications.IsChecked = true;

            ListPickerTemplateModel<Int64> frequency = new ListPickerTemplateModel<Int64>();
            frequency.Text = AppResources.SettingsPageCategoryFrequency;

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "1" + AppResources.SettingsListItemMinute,
                    60 * 1000));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "10" + AppResources.SettingsListItemMinute,
                    10 * 60 * 1000));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "1" + AppResources.SettingsListItemHour,
                    3600 * 1000));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "2" + AppResources.SettingsListItemHour,
                    2 * 3600 * 1000));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "6" + AppResources.SettingsListItemHour,
                    6 * 3600 * 1000));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "12" + AppResources.SettingsListItemHour,
                    12 * 3600 * 1000));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "1" + AppResources.SettingsListItemDay,
                    24 * 3600 * 1000));

            frequency.Value = 0;
            notifications.Children.Add(frequency);

            SwitchTemplateModel timeRange = new SwitchTemplateModel();
            timeRange.Text = AppResources.SettingsPageCategoryTimeRange;
            timeRange.IsChecked = true;

            TimePickerTemplateModel from = new TimePickerTemplateModel();
            @from.Text = AppResources.SettingsPageCategoryFrom;
            @from.Value = DateTime.Now;
            timeRange.Children.Add(@from);

            TimePickerTemplateModel to = new TimePickerTemplateModel();
            to.Text = AppResources.SettingsPageCategoryTo;
            to.Value = DateTime.Now;
            timeRange.Children.Add(to);
            notifications.Children.Add(timeRange);

            try
            {
                Unit unit = StartupConfiguration.Configuration.GetUnitById(
                    StartupConfiguration.Properties.Websites.DefaultWebsite.Id);
                List<Section> sections = unit.Sections.SectionsList;
                foreach (Section section in sections)
                {
                    CheckBoxTemplateModel category = new CheckBoxTemplateModel();
                    category.Text = section.SectionTitle;
                    category.IsChecked = section.SectionNotifications;
                    notifications.Children.Add(category);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie zgadzają się ID w plikach konfiguracyjnych.");
            }
            return notifications;
        }

        public override string ToString()
        {
            return Text;
        }


    }
}
