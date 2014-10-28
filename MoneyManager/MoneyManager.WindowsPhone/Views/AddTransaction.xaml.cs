using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Src;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Foundation;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

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
                AccountLogic.RemoveTransactionAmount(AddTransactionView.SelectedTransaction);
            }
        }

        private AddTransactionViewModel AddTransactionView
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
                Translation.GetTranslation("AccountRequiredMessage"),
                Translation.GetTranslation("AccountRequiredTitle")
                );
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));
            dialog.DefaultCommandIndex = 1;
            await dialog.ShowAsync();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            AddTransactionView.Cancel();
        }
    }
}