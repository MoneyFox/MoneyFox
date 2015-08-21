using Windows.UI.Xaml;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Foundation;

namespace MoneyManager.Windows.Views
{
    public sealed partial class AddAccountView
    {
        public AddAccountView()
        {
            InitializeComponent();
        }

        private AddAccountViewModel viewModel => ServiceLocator.Current.GetInstance<AddAccountViewModel>();

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
                viewModel.SelectedAccount.Name = Translation.GetTranslation("NoNamePlaceholderLabel");
            }

            viewModel.Save();
            ServiceLocator.Current.GetInstance<BalanceViewModel>().UpdateBalance();
        }

        //TODO: Move to ViewModel
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            viewModel.Cancel();
        }
    }
}