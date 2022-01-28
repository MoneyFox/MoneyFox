﻿using MoneyFox.Win.ViewModels.Settings;

namespace MoneyFox.Win.Pages.Settings
{
    public sealed partial class SettingsView : BaseView
    {
        private WindowsSettingsViewModel ViewModel => (WindowsSettingsViewModel)DataContext;

        public SettingsView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.SettingsVm;
        }
    }
}