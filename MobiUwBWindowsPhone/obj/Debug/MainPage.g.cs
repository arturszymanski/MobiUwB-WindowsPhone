﻿#pragma checksum "D:\programowanie\C# Projects\VS2013\MobiUwB-WindowsPhone\MobiUwBWindowsPhone\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ED1EB98F1B95D38C2774303AAD406410"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MobiUwB {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot Pivot;
        
        internal Microsoft.Phone.Controls.PivotItem StartPivot;
        
        internal Microsoft.Phone.Controls.WebBrowser MainWebBrowser;
        
        internal Microsoft.Phone.Controls.PivotItem InstitiutesPivot;
        
        internal System.Windows.Controls.ListBox InstitutesListBox;
        
        internal System.Windows.Controls.Grid ProgressIndicatorScreen;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem SettingsMenuItem;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem ContactMenuItem;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem AboutMenuItem;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem CloseMenuItem;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MobiUwB;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Pivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("Pivot")));
            this.StartPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("StartPivot")));
            this.MainWebBrowser = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("MainWebBrowser")));
            this.InstitiutesPivot = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("InstitiutesPivot")));
            this.InstitutesListBox = ((System.Windows.Controls.ListBox)(this.FindName("InstitutesListBox")));
            this.ProgressIndicatorScreen = ((System.Windows.Controls.Grid)(this.FindName("ProgressIndicatorScreen")));
            this.SettingsMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("SettingsMenuItem")));
            this.ContactMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("ContactMenuItem")));
            this.AboutMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("AboutMenuItem")));
            this.CloseMenuItem = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("CloseMenuItem")));
        }
    }
}

