using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Views;

namespace MoneyManager.UserControls
{
    public sealed partial class AddTransactionUserControl
    {
        public AddTransactionUserControl()
        {
            InitializeComponent();

            if (!ServiceLocator.Current.GetInstance<AddTransactionViewModel>().IsEdit)
            {
                SelectedTransaction.Date = DateTime.Now;
            }
        }

        private FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>().Selected; }
        }

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxAmount.Text == "0")
            {
                TextBoxAmount.Text = string.Empty;
            }

            TextBoxAmount.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (TextBoxAmount.Text == string.Empty)
            {
                TextBoxAmount.Text = "0";
            }
        }

        private void OpenSelectCurrencyDialog(object sender, RoutedEventArgs routedEventArgs)
        {
            ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>().InvocationType = InvocationType.Transaction;
            ((Frame) Window.Current.Content).Navigate(typeof (SelectCurrency));
        }

        private void ReplaceSeparatorChar(object sender, TextChangedEventArgs e)
        {
            if (e.OriginalSource == null)
            {
                return;
            }

            TextBoxAmount.Text = e.OriginalSource.ToString()
                .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            TextBoxAmount.Text = e.OriginalSource.ToString()
                .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
        }
    }
}