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

        private async void AddAccountClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new AddAccountPage());
        }
    }
}