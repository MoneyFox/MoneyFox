using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Common;

namespace MoneyManager.Views
{
    public sealed partial class AddTransaction
    {
        private readonly NavigationHelper navigationHelper;

        public AddTransaction()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);

            if (AddTransactionView.IsEdit)
            {
                ServiceLocator.Current.GetInstance<AccountDataAccess>()
                    .RemoveTransactionAmount(AddTransactionView.SelectedTransaction);
            }
        }

        public AddTransactionViewModel AddTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            if (AddTransactionView.SelectedTransaction.ChargedAccount == null)
            {
                ShowAccountRequiredMessage();
                return;
            }

            AddTransactionView.Save();
        }

        private async void ShowAccountRequiredMessage()
        {
            var dialog = new MessageDialog
                (
                Utilities.GetTranslation("AccountRequiredMessage"),
                Utilities.GetTranslation("AccountRequiredTitle")
                );
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;
            await dialog.ShowAsync();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            AddTransactionView.Cancel();
        }
    }
}