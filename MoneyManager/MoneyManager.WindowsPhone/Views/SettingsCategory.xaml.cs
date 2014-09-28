using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyManager.DataAccess;
using MoneyManager.Dialogs;
using MoneyManager.Models;

namespace MoneyManager.Views
{
    public sealed partial class SettingsCategory
    {
        private NavigationHelper navigationHelper;

        public SettingsCategory()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration

        private async void AddCategoryClick(object sender, RoutedEventArgs e)
        {
            await new CategoryDialog().ShowAsync();
        }
    }
}