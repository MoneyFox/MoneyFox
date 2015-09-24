using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using MoneyManager.Core.Helper;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class ModifyAccountView
    {
        public ModifyAccountView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<ModifyAccountViewModel>();
        }

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == "0")
            {
                TextBoxCurrentBalance.Text = string.Empty;
            }

            TextBoxCurrentBalance.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (TextBoxCurrentBalance.Text == string.Empty)
            {
                TextBoxCurrentBalance.Text = "0";
            }
            TextBoxCurrentBalance.Text = Utilities.FormatLargeNumbers(Convert.ToDouble(TextBoxCurrentBalance.Text, CultureInfo.CurrentCulture));
        }

        private void ReplaceSeparatorChar(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCurrentBalance.Text)) return;
            var cursorposition = TextBoxCurrentBalance.SelectionStart;

            TextBoxCurrentBalance.Text = TextBoxCurrentBalance.Text
                .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            if (string.IsNullOrEmpty(TextBoxCurrentBalance.Text)) return;
            TextBoxCurrentBalance.Text = TextBoxCurrentBalance.Text
                .Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

            TextBoxCurrentBalance.Select(cursorposition, 0);
        }
    }
}