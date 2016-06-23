using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views {
    public sealed partial class ModifyAccountView {
        public ModifyAccountView() {
            InitializeComponent();
            DataContext = Mvx.Resolve<ModifyAccountViewModel>();

            // code to handle bottom app bar when keyboard appears
            // workaround since otherwise the keyboard would overlay some controls
            InputPane.GetForCurrentView().Showing +=
                (s, args) => { BottomCommandBar.Visibility = Visibility.Collapsed; };
            InputPane.GetForCurrentView().Hiding += (s, args2) => {
                if (BottomCommandBar.Visibility == Visibility.Collapsed) {
                    BottomCommandBar.Visibility = Visibility.Visible;
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var viewModel = (ModifyAccountViewModel) DataContext;

            var account = e.Parameter as Account;
            if (account != null) {
                viewModel.IsEdit = true;
                viewModel.SelectedAccount = account;
            }
            else {
                viewModel.IsEdit = false;
                viewModel.SelectedAccount = new Account();
            }

            base.OnNavigatedTo(e);
        }


        private void TextBoxOnFocus(object sender, RoutedEventArgs e) {
            TextBoxCurrentBalance.SelectAll();
        }

        private void FormatTextBoxOnLostFocus(object sender, RoutedEventArgs e) {
            double amount;
            double.TryParse(TextBoxCurrentBalance.Text, out amount);
            TextBoxCurrentBalance.Text = Utilities.FormatLargeNumbers(amount);
        }
    }
}