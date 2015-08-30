using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Cirrious.CrossCore;
using MoneyManager.Core.Logic;
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

        private ModifyTransactionViewModel ModifyTransactionViewModel
            => Mvx.Resolve<ModifyTransactionViewModel>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back && ModifyTransactionViewModel.IsEdit)
            {
                AccountLogic.RemoveTransactionAmount(ModifyTransactionViewModel.SelectedTransaction);
            }

            base.OnNavigatedTo(e);
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

        //TODO: Handle in View Model
        private void DoneClick(object sender, RoutedEventArgs e)
        {
            ModifyTransactionViewModel.Save();
        }

        //TODO: Handle in View Model
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ModifyTransactionViewModel.Cancel();
        }
    }
}