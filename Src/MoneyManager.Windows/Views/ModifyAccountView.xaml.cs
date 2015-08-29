using Windows.UI.Xaml;
using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;

namespace MoneyManager.Windows.Views
{
    public sealed partial class ModifyAccountView
    {
        public ModifyAccountView()
        {
            InitializeComponent();
        }

        private ModifyAccountViewModel viewModel => Mvx.Resolve<ModifyAccountViewModel>();

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

            viewModel.SaveCommand.Execute();
        }

        //TODO: Move to ViewModel
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            viewModel.CancelCommand.Execute();
        }
    }
}