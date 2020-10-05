using MoneyFox.ViewModels.Dashboard;
using Xamarin.Forms;

namespace MoneyFox.Views.Dashboard
{
    public partial class DashboardPage : ContentPage
    {
        private DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;

        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.DashboardViewModel;
        }

        protected override async void OnAppearing()
        {
            ViewModel.Subscribe();
            await ViewModel.InitializeAsync();
        }

        protected override void OnDisappearing()
        {
            ViewModel.Unsubscribe();
        }
    }
}
