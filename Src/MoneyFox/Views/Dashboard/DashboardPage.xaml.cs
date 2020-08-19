using MoneyFox.ViewModels.Dashboard;
using Xamarin.Forms;

namespace MoneyFox.Views.Dashboard
{
    public partial class DashboardPage : ContentPage
    {
        private DashboardViewModel ViewModel => BindingContext as DashboardViewModel;

        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.DashboardViewModel;
        }

        protected override async void OnAppearing() => await ViewModel.InitializeAsync();
    }
}