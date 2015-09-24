using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using MoneyManager.Core.Helper;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class ModifyTransactionView
    {
        public ModifyTransactionView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<ModifyTransactionViewModel>();
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
            TextBoxAmount.Text = Utilities.FormatLargeNumbers(Convert.ToDouble(TextBoxAmount.Text, CultureInfo.CurrentCulture));
        }

        private void ReplaceSeparatorChar(object sender, TextChangedEventArgs e)
        { 
            if (string.IsNullOrEmpty(TextBoxAmount.Text)) return;
            var cursorposition = TextBoxAmount.SelectionStart;

            TextBoxAmount.Text = TextBoxAmount.Text
                .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            if (string.IsNullOrEmpty(TextBoxAmount.Text)) return;
            TextBoxAmount.Text = TextBoxAmount.Text
                .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            TextBoxAmount.Select(cursorposition, 0);
        }
    }
}