using MoneyFox.ViewModels.Accounts;
using Xamarin.Forms;

namespace MoneyFox.Views.Accounts
{
    public partial class AccountListPage : ContentPage
    {
        public AccountListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AccountListViewModel;
        }

        private AccountListViewModel ViewModel => (AccountListViewModel)BindingContext;

        protected override async void OnAppearing() => await ViewModel.OnAppearingAsync();
    }
}