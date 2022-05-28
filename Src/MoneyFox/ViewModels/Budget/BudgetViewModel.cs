namespace MoneyFox.ViewModels.Budget
{

    using CommunityToolkit.Mvvm.ComponentModel;

    public sealed class BudgetViewModel : ObservableObject
    {
        private string name = null!;

        private double currentSpending;

        private double spendingLimit;
        public int Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(field: ref name, newValue: value);
        }

        public double CurrentSpending
        {
            get => currentSpending;
            set => SetProperty(field: ref currentSpending, newValue: value);
        }

        public double SpendingLimit
        {
            get => spendingLimit;
            set => SetProperty(field: ref spendingLimit, newValue: value);
        }
    }

}
