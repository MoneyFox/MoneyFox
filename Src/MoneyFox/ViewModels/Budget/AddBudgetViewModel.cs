namespace MoneyFox.ViewModels.Budget
{

    using CommunityToolkit.Mvvm.ComponentModel;

    public sealed class AddBudgetViewModel : ObservableRecipient
    {


        private BudgetViewModel selectedBudget = new BudgetViewModel();
        public BudgetViewModel SelectedBudget
        {
            get => selectedBudget;
            set => SetProperty(field: ref selectedBudget, newValue: value);
        }
    }

}
