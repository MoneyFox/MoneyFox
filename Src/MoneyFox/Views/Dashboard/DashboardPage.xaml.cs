namespace MoneyFox.Views.Dashboard
{

    using ViewModels.Dashboard;
    using Xamarin.Forms;

    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.DashboardViewModel;
        }

        private DashboardViewModel ViewModel => (DashboardViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            await ViewModel.InitializeAsync();
        }
    }

}
