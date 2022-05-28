namespace MoneyFox.ViewModels.Budget
{

    using CommunityToolkit.Mvvm.ComponentModel;

    public sealed class BudgetViewModel : ObservableObject
    {
        private string name = null!;

        public string Name
        {
            get => name;
            set => SetProperty(field: ref name, newValue: value);
        }

        private double currentSpending;

        public double CurrentSpending
        {
            get => currentSpending;
            set => SetProperty(field: ref currentSpending, newValue: value);
        }

        private double spendingLimit;

        public double SpendingLimit
        {
            get => spendingLimit;
            set => SetProperty(field: ref spendingLimit, newValue: value);
        }
    }

}
