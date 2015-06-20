#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Net.NetworkInformation;
using MobiUwB.Resources;
using MobiUwB.Utilities;

#endregion

namespace MobiUwB.Connection
{
    public class InternetChecker : BackgroundWorker
    {
        private readonly BitmapImage _3GBitmapImage;
        private readonly BitmapImage _4GBitmapImage;
        private readonly BitmapImage _wiFiBitmapImage;
        private readonly BitmapImage _noConnectionBitmapImage;
        private readonly BitmapImage _pcBitmapImage;
        private readonly BitmapImage _unknownBitmapImage;

        private readonly Uri _3GImageUri;
        private readonly Uri _4GImageUri;
        private readonly Uri _wiFiImageUri;
        private readonly Uri _noConnectionImageUri;
        private readonly Uri _pcImageUri;
        private readonly Uri _unknownImageUri;

        private Boolean _shouldStop;
        private InternetConnectionType? _internetConnectionType;


        public InternetChecker()
        {
            _3GImageUri = 
                new Uri(
                    "Assets\\ToastIcons\\3g.png", 
                    UriKind.Relative);

            _4GImageUri =
                new Uri(
                    "Assets\\ToastIcons\\4g.png",
                    UriKind.Relative);

            _wiFiImageUri =
                new Uri(
                    "Assets\\ToastIcons\\wifi.png",
                    UriKind.Relative);

            _noConnectionImageUri =
                new Uri(
                    "Assets\\ToastIcons\\no_connection.png",
                    UriKind.Relative);

            _pcImageUri =
                new Uri(
                    "Assets\\ToastIcons\\pc.png",
                    UriKind.Relative);

            _unknownImageUri =
                new Uri(
                    "Assets\\ToastIcons\\unknown.png",
                    UriKind.Relative);

            _3GBitmapImage = new BitmapImage(_3GImageUri);
            _4GBitmapImage = new BitmapImage(_4GImageUri);
            _wiFiBitmapImage = new BitmapImage(_wiFiImageUri);
            _noConnectionBitmapImage = new BitmapImage(_noConnectionImageUri);
            _pcBitmapImage = new BitmapImage(_pcImageUri);
            _unknownBitmapImage = new BitmapImage(_unknownImageUri);

            WorkerReportsProgress = true;
            DoWork += InternetChecker_DoWork;
            ProgressChanged += InternetChecker_ProgressChanged;
        }


        public void StartChecker()
        {
            RunWorkerAsync();
            _shouldStop = false;
        }

        public void StopChecker()
        {
            _shouldStop = false;
        }

        void InternetChecker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            InternetConnectionType connectionType = (InternetConnectionType) e.ProgressPercentage;
            UseResponse(connectionType);
        }

        void InternetChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<InternetConnectionType> currentConnectionTypes = new List<InternetConnectionType>();
            while (!_shouldStop)
            {
                var currentList = new NetworkInterfaceList().
                    Where(i => i.InterfaceState == ConnectState.Connected).
                    Select(i => i.InterfaceSubtype);

                foreach (NetworkInterfaceSubType interfaceSubType in currentList)
                {
                    switch (interfaceSubType)
                    {
                        case NetworkInterfaceSubType.Cellular_EVDO:
                        case NetworkInterfaceSubType.Cellular_3G:
                        case NetworkInterfaceSubType.Cellular_HSPA:
                        case NetworkInterfaceSubType.Cellular_EVDV:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.Unknown);
                            break;
                        }
                        case NetworkInterfaceSubType.Cellular_GPRS:
                        case NetworkInterfaceSubType.Cellular_1XRTT:
                        case NetworkInterfaceSubType.Cellular_EDGE:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.G3);
                            break;
                        }
                        case NetworkInterfaceSubType.WiFi:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.WiFi);
                            break;
                        }
                        case NetworkInterfaceSubType.Desktop_PassThru:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.Cable);
                            break;
                        }
                        case NetworkInterfaceSubType.Cellular_EHRPD:
                        case NetworkInterfaceSubType.Cellular_LTE:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.G4);
                            break;
                        }
                        case NetworkInterfaceSubType.Unknown:
                        {
                            currentConnectionTypes.Add(InternetConnectionType.Unknown);
                            break;
                        }
                        default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                InternetConnectionType connectionType = 
                    AnalizeConnectionTypes(
                        currentConnectionTypes);

                DetermineReportProgress(connectionType);

                currentConnectionTypes.Clear();
            }
        }

        private void DetermineReportProgress(InternetConnectionType currConnectionType)
        {
            if (_internetConnectionType != null && _internetConnectionType != currConnectionType)
            {
                ReportProgress((Int32)currConnectionType);
            }
            _internetConnectionType = currConnectionType;
        }

        private InternetConnectionType AnalizeConnectionTypes(List<InternetConnectionType> connectionTypes)
        {
            if (connectionTypes.Contains(InternetConnectionType.WiFi))
            {
                return InternetConnectionType.WiFi;
            }
            if (connectionTypes.Contains(InternetConnectionType.Cable))
            {
                return InternetConnectionType.Cable;
            }
            if (connectionTypes.Contains(InternetConnectionType.G4))
            {
                return InternetConnectionType.G4;
            }
            if (connectionTypes.Contains(InternetConnectionType.G3))
            {
                return InternetConnectionType.G3;
            }
            if (connectionTypes.Contains(InternetConnectionType.G2))
            {
                return InternetConnectionType.G2;
            }
            return InternetConnectionType.NoConnection;
        }

        public void UseResponse(InternetConnectionType connectionType)
        {
            String title = AppResources.InternetCheckerToastTitle;
            String message;
            BitmapImage bitmapImage;

            switch (connectionType)
            {
                case InternetConnectionType.Cable:
                {
                    message = AppResources.InternetCheckerToastPc;
                    bitmapImage = _pcBitmapImage;
                    break;
                }
                case InternetConnectionType.WiFi:
                {
                    message = AppResources.InternetCheckerToastWiFi;
                    bitmapImage = _wiFiBitmapImage;
                    break;
                }
                case InternetConnectionType.G2:
                {
                    message = AppResources.InternetCheckerToast2g;
                    bitmapImage = _unknownBitmapImage;
                    break;
                }
                case InternetConnectionType.G3:
                {
                    message = AppResources.InternetCheckerToastMobile3G;
                    bitmapImage = _3GBitmapImage;
                    break;
                }
                case InternetConnectionType.G4:
                {
                    message = AppResources.InternetCheckerToastMobile4G;
                    bitmapImage = _4GBitmapImage;
                    break;
                }
                case InternetConnectionType.Unknown:
                {
                    message = AppResources.InternetCheckerToastUnknown;
                    bitmapImage = _unknownBitmapImage;
                    break;
                }
                case InternetConnectionType.NoConnection:
                {
                    message = AppResources.InternetCheckerToastNoConnection;
                    bitmapImage = _noConnectionBitmapImage;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException("connectionType", connectionType, null);
                }
            }

            ToastManager.ShowToast(title, message, bitmapImage);
        }
    }
}
