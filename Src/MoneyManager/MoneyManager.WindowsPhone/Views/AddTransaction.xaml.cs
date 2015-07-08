using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Foundation;

namespace MoneyManager.Views
{
    public sealed partial class AddTransaction
    {
        public AddTransaction()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
        }

        private AddTransactionViewModel AddTransactionView => ServiceLocator.Current.GetInstance<AddTransactionViewModel>();

        private NavigationHelper NavigationHelper { get; }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back && AddTransactionView.IsEdit)
            {
                await AccountLogic.RemoveTransactionAmount(AddTransactionView.SelectedTransaction);
            }

            base.OnNavigatedTo(e);
        }

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e) {
            if (TextBoxAmount.Text == "0") {
                TextBoxAmount.Text = string.Empty;
            }

            TextBoxAmount.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e) {
            if (TextBoxAmount.Text == string.Empty) {
                TextBoxAmount.Text = "0";
            }
        }

        private void OpenSelectCurrencyDialog(object sender, RoutedEventArgs routedEventArgs) {
            ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>().InvocationType = InvocationType.Transaction;
            ((Frame)Window.Current.Content).Navigate(typeof(SelectCurrency));
        }

        private void ReplaceSeparatorChar(object sender, TextChangedEventArgs e) {
            if (e.OriginalSource == null) {
                return;
            }

            TextBoxAmount.Text = e.OriginalSource.ToString()
                .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            TextBoxAmount.Text = e.OriginalSource.ToString()
                .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
        }


        private void DoneClick(object sender, RoutedEventArgs e)
        {
            AddTransactionView.Save();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            AddTransactionView.Cancel();
        }
    }
}