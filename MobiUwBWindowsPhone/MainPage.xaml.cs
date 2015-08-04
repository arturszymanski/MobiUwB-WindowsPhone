

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MobiUwB.StartupConfig;
using MobiUwB.Utilities;
using SharedCode.Parsers;
using SharedCode;
using SharedCode.Utilities;

#endregion

namespace MobiUwB
{
    public partial class MainPage : PhoneApplicationPage
    {

        public MainPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnabledForApplicationBarImageButtons(false);
            ProgressIndicatorScreen.Visibility = Visibility.Visible;

            Uri defaultWebsiteUri = new Uri(StartupConfiguration.Properties.Websites.DefaultWebsite.Url);

            MainWebBrowser.Navigate(
                defaultWebsiteUri);

            List<InstitiuteModel> institiutesLst = 
                ParserFactory.GenerateInstituteModels(
                    StartupConfiguration.Properties.Websites.WebsiteList, 
                    @"Assets/logouwb.png");

            InstitutesListBox.DataContext = institiutesLst;

            InstitutesListBox.SelectedItem =
                ParserFactory.FindWrapperBy(
                    StartupConfiguration.Properties.Websites.DefaultWebsite,
                    institiutesLst); 
        }

        private void Initialize()
        {
            StartPivot.Tag = PivotItemType.Start;
            InstitiutesPivot.Tag = PivotItemType.Institiutes;
        }


        private void SetEnabledForApplicationBarImageButtons(Boolean isEnabled)
        {
            foreach (ApplicationBarIconButton button in ApplicationBar.Buttons)
            {
                button.IsEnabled = isEnabled;
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            SetEnabledForApplicationBarImageButtons(false);
            ProgressIndicatorScreen.Visibility = Visibility.Visible;
            MainWebBrowser.Opacity = 0.1;
            MainWebBrowser.Navigate(MainWebBrowser.Source);
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            string pingPage = StartupConfiguration.Properties.Websites.DefaultWebsite.Ping;
            if (MainWebBrowser.Source != null && 
                MainWebBrowser.Source.OriginalString != pingPage)
            {
                e.Cancel = true;
                MainWebBrowser.GoBack();
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }


        private void Settings_MenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings/SettingsPage.xaml", UriKind.Relative));
        }


        private void Contact_MenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Contact/ContactPage.xaml", UriKind.Relative));
        }


        private void About_MenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About/AboutPage.xaml", UriKind.Relative));
        }


        private void Close_MenuItem_Click(object sender, EventArgs e)
        {
            Application.Current.Terminate(); 
        }


        private void Institutes_ListBox_SelectionChanged(
            object sender, 
            SelectionChangedEventArgs e)
        {
            if (InstitutesListBox.SelectedItem == null)
            {
                return;
            }
            InstitiuteModel selectedItem = 
                (InstitiuteModel)InstitutesListBox.SelectedItem;

            if (!selectedItem.Website.Equals(
                StartupConfiguration.Properties.Websites.DefaultWebsite))
            {
                StartupConfiguration.Properties.Websites.DefaultWebsite = 
                    selectedItem.Website;

                Globals.CurrentUnitId = selectedItem.Website.Id;
                UnitIdStorer unitIdStorer = new UnitIdStorer();
                unitIdStorer.RunWorkerAsync(Globals.CurrentUnitId);

                try
                {
                    App.XmlParser.Serialize(
                        StartupConfiguration.Properties,
                        StartupConfiguration.PropertiesFileName);
                }
                catch (Exception exception)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Error Message: ");
                    stringBuilder.Append(exception.Message);
                    stringBuilder.Append("\nInner Exception: ");
                    stringBuilder.Append(exception.InnerException);
                    stringBuilder.Append("\nCallStack: ");
                    stringBuilder.Append(exception.StackTrace);

                    Debug.WriteLine(stringBuilder.ToString());
                }
            }

            Pivot.SelectedItem = StartPivot;
            MainWebBrowser.Navigate(new Uri(selectedItem.Page));

            SetEnabledForApplicationBarImageButtons(false);
            ProgressIndicatorScreen.Visibility = Visibility.Visible;
        }

        private void Main_WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            EnableMainScreen();
        }

        private void MainWebBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            EnableMainScreen();
        }

        private void EnableMainScreen()
        {
            MainWebBrowser.Opacity = 1;
            SetEnabledForApplicationBarImageButtons(true);
            ProgressIndicatorScreen.Visibility = Visibility.Collapsed;
        }

        private void Pivot_SelectionChanged(
            object sender, 
            SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            PivotItem selectedItem = pivot.SelectedItem as PivotItem;
            if (selectedItem != null)
            {
                PivotItemType? piwotType = selectedItem.Tag as PivotItemType?;
                switch (piwotType)
                {
                    case PivotItemType.Institiutes:
                    {
                        SetEnabledForApplicationBarImageButtons(false);
                        break;
                    }
                    case PivotItemType.Start:
                    {
                        SetEnabledForApplicationBarImageButtons(true);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }

        private void MainWebBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            Debug.WriteLine("e.Uri");
            Debug.WriteLine(e.Uri);
            if(e.Uri.AbsolutePath.EndsWith(".pdf") &&
                !e.Uri.AbsolutePath.StartsWith("http://docs.google.com/gview?embedded=true&url="))
            {
                MainWebBrowser.Navigate(new Uri("http://docs.google.com/gview?embedded=true&url=" + e.Uri));
                e.Cancel = true;
            }
        }

        private void MainWebBrowser_OnNavigated(object sender, NavigationEventArgs e)
        {
        }
    }
}