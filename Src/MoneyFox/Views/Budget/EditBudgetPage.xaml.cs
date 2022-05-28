namespace MoneyFox.Views.Budget
{

    using Xamarin.Forms;

    public partial class EditBudgetPage : ContentPage
    {
        private readonly int budgetId;

        public EditBudgetPage(int budgetId)
        {
            InitializeComponent();
            this.budgetId = budgetId;
        }
    }

}
