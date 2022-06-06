namespace MoneyFox.Views.Budget
{

    using ViewModels.Budget;
    using Xamarin.Forms;

    public partial class BudgetListPage : ContentPage
    {
        public BudgetListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.BudgetListViewModel;
        }

        private BudgetListViewModel ViewModel => (BudgetListViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.InitializeCommand.ExecuteAsync(null);
        }
    }

}
