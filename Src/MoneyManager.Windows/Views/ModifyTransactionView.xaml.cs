using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
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