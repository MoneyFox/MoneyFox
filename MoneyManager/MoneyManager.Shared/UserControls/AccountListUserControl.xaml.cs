using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.ViewModels.Data;
using MoneyManager.Views;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MoneyManager.UserControls
{
    public sealed partial class AccountListUserControl
    {
        private readonly Parameters parameters = new Parameters();

        public AccountListUserControl()
        {
            InitializeComponent();
        }

        private AccountViewModel accountViewModel
        {
            get { return new ViewModelLocator().AccountViewModel; }
        }

        private void AccountList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var account = element.DataContext as Account;
            if (account == null) return;

            var dialog = new MessageDialog(Utilities.GetTranslation("DeleteQuestionMessage"),
                Utilities.GetTranslation("DeleteAccountQuestionMessage"));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            var result = await dialog.ShowAsync();

            if (result.Label == Utilities.GetTranslation("YesLabel"))
            {
                accountViewModel.Delete(account);
            }
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            accountViewModel.SelectedAccount = element.DataContext as Account;
            parameters.Edit = true;
            ((Frame)Window.Current.Content).Navigate(typeof(AddAccount), parameters);
        }

        private void AccountList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountList.SelectedItem != null)
            {
                accountViewModel.SelectedAccount = AccountList.SelectedItem as Account;
                ((Frame)Window.Current.Content).Navigate(typeof(TransactionList));
                AccountList.SelectedItem = null;
            }
        }
    }
}