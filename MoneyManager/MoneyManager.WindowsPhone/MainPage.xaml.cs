using MoneyManager.Models;
using MoneyManager.Src;
using MoneyManager.ViewModels;
using MoneyManager.Views;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private Account selectedAccount
        {
            set { new ViewModelLocator().Main.SelectedAccount = value; }
        }

        private FinancialTransaction selectedTransaction
        {
            set { new ViewModelLocator().Main.SelectedTransaction = value; }
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddAccount));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsOverview));
        }
    }
}