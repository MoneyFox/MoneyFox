
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Foundation;

namespace MoneyManager.Views {
    public sealed partial class AddTransaction {
        private readonly NavigationHelper navigationHelper;

        public AddTransaction() {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        private AddTransactionViewModel AddTransactionView {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public NavigationHelper NavigationHelper {
            get { return navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.NavigationMode != NavigationMode.Back && AddTransactionView.IsEdit) {
                //TODO:Refactor
                //await AccountLogic.RemoveTransactionAmount(AddTransactionView.SelectedTransaction);
            }

            base.OnNavigatedTo(e);
        }

        private void DoneClick(object sender, RoutedEventArgs e) {
            AddTransactionView.Save();
        }

        private void CancelClick(object sender, RoutedEventArgs e) {
            AddTransactionView.Cancel();
        }
    }
}