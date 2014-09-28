using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Views;

namespace MoneyManager.UserControls
{
    public sealed partial class AddTransactionUserControl
    {
        public AddTransactionUserControl()
        {
            InitializeComponent();

            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Date = DateTime.Now;
        }

        private void ResetCategory(object sender, TappedRoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Category = null;
        }

        private void OpenSelectCategoryDialog(object sender, RoutedEventArgs routedEventArgs)
        {
            ((Frame) Window.Current.Content).Navigate(typeof (SelectCategory));
        }
    }
}