using MoneyFox.ViewModels.Accounts;
using Xamarin.Forms;

namespace MoneyFox.Views.Accounts
{
    public partial class AccountListPage : ContentPage
    {
        private AccountListViewModel ViewModel => (AccountListViewModel) BindingContext;

        public AccountListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AccountListViewModel;
        }

        protected override async void OnAppearing() => await ViewModel.OnAppearingAsync();
    }
}