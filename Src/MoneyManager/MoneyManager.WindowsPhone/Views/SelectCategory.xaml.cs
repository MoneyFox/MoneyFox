using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Common;
using MoneyManager.Dialogs;

namespace MoneyManager.Views {
    public sealed partial class SelectCategory {
        private readonly NavigationHelper _navigationHelper;

        public SelectCategory() {
            InitializeComponent();

            _navigationHelper = new NavigationHelper(this);
        }

        private async void AddCategory(object sender, RoutedEventArgs e) {
            await new CategoryDialog().ShowAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            _navigationHelper.OnNavigatedFrom(e);
        }
    }
}