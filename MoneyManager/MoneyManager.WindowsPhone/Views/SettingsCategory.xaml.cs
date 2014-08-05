using MoneyManager.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

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

        private void AddCategoryOnClick(object sender, RoutedEventArgs e)
        {
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