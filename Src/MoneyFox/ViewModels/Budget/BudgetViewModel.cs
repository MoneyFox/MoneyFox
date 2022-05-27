namespace MoneyFox.ViewModels.Budget
{

    using CommunityToolkit.Mvvm.ComponentModel;

    internal sealed class BudgetViewModel : ObservableObject
    {
        private string name = null!;

        public string Name
        {
            get => name;
            set => SetProperty(field: ref name, newValue: value);
        }
    }

}
