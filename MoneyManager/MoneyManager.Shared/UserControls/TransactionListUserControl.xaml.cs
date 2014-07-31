using MoneyManager.Models;
using MoneyManager.ViewModels;
using MoneyManager.Views;
using MoneyTracker.Models;
using MoneyTracker.Src;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MoneyManager.UserControls
{
    public sealed partial class TransactionListUserControl
    {
        private readonly Parameters parameters = new Parameters();

        public TransactionListUserControl()
        {
            InitializeComponent();
        }

        public FinancialTransaction SelectedTransaction
        {
            get { return new ViewModelLocator().TransactionViewModel.SelectedTransaction; }
            set { new ViewModelLocator().TransactionViewModel.SelectedTransaction = value; }
        }

        private void TransactionList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var transaction = element.DataContext as FinancialTransaction;
            if (transaction == null) return;

            var dialog = new MessageDialog(Utilities.GetTranslation("DeleteTransactionQuestionMessage"),
                Utilities.GetTranslation("DeleteQuestionTitle"));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Utilities.GetTranslation("NoLabel")));
            dialog.DefaultCommandIndex = 1;

            var result = await dialog.ShowAsync();

            if (result.Label == Utilities.GetTranslation("YesLabel"))
            {
                App.TransactionViewModel.Delete(transaction);
            }
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            SelectedTransaction = element.DataContext as FinancialTransaction;
            SetParametersAndNavigate();
        }

        private void TransactionListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionList.SelectedItem != null)
            {
                SelectedTransaction = TransactionList.SelectedItem as FinancialTransaction;
                SetParametersAndNavigate();
                TransactionList.SelectedItem = null;
            }
        }

        private void SetParametersAndNavigate()
        {
            parameters.Edit = true;
            parameters.TransactionType = (TransactionType)SelectedTransaction.Type;
            ((Frame)Window.Current.Content).Navigate(typeof(AddTransaction), parameters);
        }
    }
}