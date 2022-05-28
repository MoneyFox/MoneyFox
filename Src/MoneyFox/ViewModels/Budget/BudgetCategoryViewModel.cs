namespace MoneyFox.ViewModels.Budget
{

    using CommunityToolkit.Mvvm.ComponentModel;

    public sealed class BudgetCategoryViewModel : ObservableObject
    {
        public BudgetCategoryViewModel(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }

        public int CategoryId { get; }

        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(field: ref name, newValue: value);
        }
    }

}
