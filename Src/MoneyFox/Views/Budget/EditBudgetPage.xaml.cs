namespace MoneyFox.Views.Budget
{

    using System.Threading.Tasks;
    using Core.Resources;
    using ViewModels.Budget;
    using Xamarin.Forms;

    public partial class EditBudgetPage : ContentPage
    {
        private readonly int budgetId;

        public EditBudgetPage(int budgetId)
        {
            InitializeComponent();
            this.budgetId = budgetId;
            BindingContext = App.GetViewModel<EditBudgetViewModel>();
            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(() => ViewModel.SaveBudgetCommand.Execute(null)),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);
        }

        private EditBudgetViewModel ViewModel => (EditBudgetViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            await ViewModel.InitializeCommand.ExecuteAsync(budgetId);
        }
    }

}
