using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;

namespace MoneyManager.Windows.Views
{
    public sealed partial class AddAccountView
    {
        public AddAccountView()
        {
            InitializeComponent();
        }

        private AddAccountViewModel viewModel => Mvx.Resolve<AddAccountViewModel>();

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
        }

        //TODO: Move to ViewModel
        private void DoneClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.SelectedAccount.Name))
            {
                viewModel.SelectedAccount.Name = Strings.NoNamePlaceholderLabel;
            }

            viewModel.Save();
            Mvx.Resolve<BalanceViewModel>().UpdateBalance();
        }

        //TODO: Move to ViewModel
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            viewModel.Cancel();
        }
    }
}