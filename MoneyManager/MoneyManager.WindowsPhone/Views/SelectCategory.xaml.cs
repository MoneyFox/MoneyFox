#region

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;
using MoneyManager.Dialogs;

#endregion

namespace MoneyManager.Views {
    public sealed partial class SelectCategory {
        private readonly NavigationHelper navigationHelper;

        public SelectCategory() {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
        }

        private async void AddCategory(object sender, RoutedEventArgs e) {
            await new CategoryDialog().ShowAsync();
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}