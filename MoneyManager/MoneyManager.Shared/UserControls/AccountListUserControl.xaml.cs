using MoneyTracker.Models;
using MoneyTracker.Src;
using MoneyTracker.Views;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MoneyTracker.UserControls
{
    public sealed partial class AccountListUserControl
    {
        private readonly Parameters parameters = new Parameters();

        public AccountListUserControl()
        {
            InitializeComponent();
            DataContext = App.AccountViewModel;

            TextBlockCurrency.Text = App.Settings.Currency;
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
                App.AccountViewModel.Delete(account);
            }
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            App.AccountViewModel.SelectedAccount = element.DataContext as Account;
            parameters.Edit = true;
            ((Frame)Window.Current.Content).Navigate(typeof(AddAccount), parameters);
        }

        private void AccountList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountList.SelectedItem != null)
            {
                App.AccountViewModel.SelectedAccount = AccountList.SelectedItem as Account;
                ((Frame)Window.Current.Content).Navigate(typeof(TransactionList));
                AccountList.SelectedItem = null;
            }
        }
    }
}