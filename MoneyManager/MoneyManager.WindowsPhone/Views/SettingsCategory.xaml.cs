#region

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Dialogs;

#endregion

namespace MoneyManager.Views
{
    public sealed partial class SettingsCategory
    {
        private readonly NavigationHelper navigationHelper;

        public SettingsCategory()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
            ServiceLocator.Current.GetInstance<CategoryListViewModel>().IsSettingCall = true;
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private async void AddCategoryClick(object sender, RoutedEventArgs e)
        {
            await new CategoryDialog().ShowAsync();
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

    }
}