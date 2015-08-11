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
using SharedCode;
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

        private String _id;
        [DataMember]
        public String ID
        {
            get { return _id; }
            private set { _id = value; }
        }


        public TemplateModel()
        {
            _children = new List<TemplateModel>();
        }

        public TemplateModel GetDefaults()
        {
            SwitchTemplateModel notifications = new SwitchTemplateModel();
            notifications.ID = Defaults.NotificationsActiveId;
            notifications.Text = AppResources.SettingsPageCategoryNotifications;
            notifications.IsChecked = true;

            ListPickerTemplateModel<Int64> frequency = new ListPickerTemplateModel<Int64>();
            frequency.ID = Defaults.FrequencyId;
            frequency.Text = AppResources.SettingsPageCategoryFrequency;

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "1 " + AppResources.SettingsListItemMinute,Defaults.Frequencies[0]
                    ));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "10 " + AppResources.SettingsListItemMinute,Defaults.Frequencies[1]
                    ));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "1 " + AppResources.SettingsListItemHour,Defaults.Frequencies[2]
                    ));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "2 " + AppResources.SettingsListItemHour,Defaults.Frequencies[3]
                    ));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "6 " + AppResources.SettingsListItemHour,Defaults.Frequencies[4]
                    ));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "12 " + AppResources.SettingsListItemHour,Defaults.Frequencies[5]
                    ));

            frequency.Items.Add(
                new ListPickerTemplateModel<long>.ListPickerItem(
                    "1 " + AppResources.SettingsListItemDay,Defaults.Frequencies[6]
                    ));

            frequency.Value = Defaults.DefaultFrequencyIndex;
            notifications.Children.Add(frequency);

            SwitchTemplateModel timeRange = new SwitchTemplateModel();
            timeRange.ID = Defaults.TimeRangeId;
            timeRange.Text = AppResources.SettingsPageCategoryTimeRange;
            timeRange.IsChecked = Defaults.TimeRangeDefaultValue;

            TimePickerTemplateModel from = new TimePickerTemplateModel();
            @from.ID = Defaults.FromId;
            @from.Text = AppResources.SettingsPageCategoryFrom;
            @from.Value = Defaults.FromDefaultValue;
            timeRange.Children.Add(@from);

            TimePickerTemplateModel to = new TimePickerTemplateModel();
            to.ID = Defaults.ToId;
            to.Text = AppResources.SettingsPageCategoryTo;
            to.Value = Defaults.ToDefaultValue;
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
                    category.ID = section.SectionId;
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
