namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using Extensions;
    using Xamarin.Forms;

    public sealed class AddBudgetViewModel : ObservableRecipient, IRecipient<CategorySelectedMessage>
    {
        public AddBudgetViewModel()
        {
            WeakReferenceMessenger.Default.Register(this);
        }

        private BudgetViewModel selectedBudget = new BudgetViewModel();

        public BudgetViewModel SelectedBudget
        {
            get => selectedBudget;
            set => SetProperty(field: ref selectedBudget, newValue: value);
        }

        public ObservableCollection<BudgetCategoryViewModel> SelectedCategories { get; set; } = new ObservableCollection<BudgetCategoryViewModel>();

        public AsyncRelayCommand SelectCategoryCommand => new AsyncRelayCommand(SelectCategory);

        public void Receive(CategorySelectedMessage message)
        {
            var categorySelectedDataSet = message.Value;
            SelectedCategories.Add(new BudgetCategoryViewModel(categoryId: categorySelectedDataSet.CategoryId, name: categorySelectedDataSet.Name));
        }

        private static async Task SelectCategory()
        {
            await Shell.Current.GoToModalAsync(ViewModelLocator.SelectCategoryRoute);
        }
    }

}
