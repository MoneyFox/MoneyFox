namespace MoneyFox.Views.Accounts
{

    using ViewModels.Accounts;
    using Xamarin.Forms;

    public partial class AccountListPage : ContentPage
    {
        public AccountListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AccountListViewModel;
        }

        private AccountListViewModel ViewModel => (AccountListViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            await ViewModel.OnAppearingAsync();
        }
    }

}
