#region

using System;
using Microsoft.Phone.Controls;
using MobiUwB.Controls.RoundButtons;
using MobiUwB.Resources;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace MobiUwB.Views.SomethingWrong
{
    public partial class SomethingWrongPage : PhoneApplicationPage
    {
        public SomethingWrongPage()
        {
            InitializeComponent();
            RoundButtonModel roundButtonModel = 
                new RoundButtonModel(
                    AppResources.SomethingWrongPageRefreshRoundButtonText,
                    "\U0000E117",
                    null);

            RefreshRoundButton.DataContext = roundButtonModel;
        }

        private void RefreshRoundButton_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(
                new Uri("/Views/SplashScreen/SplashScreenPage.xaml",
                    UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }
    }
}