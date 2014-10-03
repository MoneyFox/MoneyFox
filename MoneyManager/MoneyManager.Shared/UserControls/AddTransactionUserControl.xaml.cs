using System;
using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;


namespace MoneyManager.UserControls
{
    public sealed partial class AddTransactionUserControl
    {
        public AddTransactionUserControl()
        {
            InitializeComponent();

            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Date = DateTime.Now;
        }

        private void RemoveZeroOnFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (TextBoxAmount.Text == "0")
            {
                TextBoxAmount.Text = String.Empty;
            }

            TextBoxAmount.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (TextBoxAmount.Text == String.Empty)
            {
                TextBoxAmount.Text = "0";
            }
        }
    }
}