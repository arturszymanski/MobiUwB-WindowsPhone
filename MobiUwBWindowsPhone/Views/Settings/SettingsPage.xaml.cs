#region

using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MobiUwB.DataAccess;
using MobiUwB.StartupConfig;
using MobiUwB.Views.Settings.Templates;
using SharedCode.DataManagment;

#endregion

namespace MobiUwB.Views.Settings
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        static Random _radnom = new Random();
        private readonly DataManager _dataManager;

        public SettingsPage()
        {
            InitializeComponent();
            ReadData readData = new ReadData();
            WriteData writeData = new WriteData();
            _dataManager = new DataManager(readData, writeData);
        }

        private void SettingsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Expander.DataContext = _dataManager.RestoreData<TemplateModel>(
                StartupConfiguration.Properties.Websites.DefaultWebsite.Id);
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataManager.StoreData(
                (TemplateModel)Expander.DataContext, 
                StartupConfiguration.Properties.Websites.DefaultWebsite.Id);
        }
    }
}