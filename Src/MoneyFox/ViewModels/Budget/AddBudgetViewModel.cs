namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.UseCases.BudgetCreation;
    using Core.Common.Messages;
    using Extensions;
    using MediatR;
    using Xamarin.Forms;

    public sealed class AddBudgetViewModel : ObservableRecipient, IRecipient<CategorySelectedMessage>
    {
        private readonly ISender sender;
        private BudgetViewModel selectedBudget = new BudgetViewModel();

        public AddBudgetViewModel(ISender sender)
        {
            this.sender = sender;
            WeakReferenceMessenger.Default.Register(this);
        }

        public BudgetViewModel SelectedBudget
        {
            get => selectedBudget;
            set => SetProperty(field: ref selectedBudget, newValue: value);
        }

        public ObservableCollection<BudgetCategoryViewModel> SelectedCategories { get; set; } = new ObservableCollection<BudgetCategoryViewModel>();

        public AsyncRelayCommand SaveBudgetCommand => new AsyncRelayCommand(SaveBudgetAsync);

        public void Receive(CategorySelectedMessage message)
        {
            var categorySelectedDataSet = message.Value;
            if (SelectedCategories.Any(c => c.CategoryId == message.Value.CategoryId) is false)
            {
                SelectedCategories.Add(new BudgetCategoryViewModel(categoryId: categorySelectedDataSet.CategoryId, name: categorySelectedDataSet.Name));
            }
        }

        private async Task SaveBudgetAsync()
        {
            var query = new CreateBudget.Query(
                name: SelectedBudget.Name,
                spendingLimit: SelectedBudget.SpendingLimit,
                categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

            await sender.Send(query);
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }

}
