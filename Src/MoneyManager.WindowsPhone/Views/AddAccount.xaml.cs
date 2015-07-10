using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.Common;
using MoneyManager.Foundation;

namespace MoneyManager.Views
{
    public sealed partial class AddAccount
    {
        public AddAccount()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
        }

        private AddAccountViewModel viewModel => ServiceLocator.Current.GetInstance<AddAccountViewModel>();

        private void RemoveZeroOnFocus(object sender, RoutedEventArgs e) {
            if (TextBoxCurrentBalance.Text == "0") {
                TextBoxCurrentBalance.Text = string.Empty;
            }

            TextBoxCurrentBalance.SelectAll();
        }

        private void AddZeroIfEmpty(object sender, RoutedEventArgs e) {
            if (TextBoxCurrentBalance.Text == string.Empty) {
                TextBoxCurrentBalance.Text = "0";
            }
        }

        //TODO: Move to ViewModel
        private void OpenSelectCurrencyDialog(object sender, RoutedEventArgs e) {
            ((Frame)Window.Current.Content).Navigate(typeof(SelectCurrency));
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

        #region NavigationHelper registration

        private NavigationHelper NavigationHelper { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion NavigationHelper registration
    }
}