namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.Queries.BudgetListLoading;
    using Core.Common.Extensions;
    using Core.Common.Messages;
    using MediatR;
    using Views.Budget;

    public sealed class BudgetListViewModel : BaseViewModel, IRecipient<ReloadMessage>
    {
        private readonly ISender sender;

        public BudgetListViewModel(ISender sender)
        {
            this.sender = sender;
            WeakReferenceMessenger.Default.Register(this);
        }

        public bool HasBudgets => Budgets.Any();

        public ObservableCollection<BudgetListItemViewModel> Budgets { get; } = new ObservableCollection<BudgetListItemViewModel>();

        public decimal BudgetedAmount => Budgets.Sum(b => b.SpendingLimit);

        public AsyncRelayCommand InitializeCommand => new AsyncRelayCommand(Initialize);

        public AsyncRelayCommand GoToAddBudgetCommand => new AsyncRelayCommand(GoToAddBudget);

        public AsyncRelayCommand<BudgetListItemViewModel> EditBudgetCommand => new AsyncRelayCommand<BudgetListItemViewModel>(EditBudgetAsync);

        public async void Receive(ReloadMessage message)
        {
            await Initialize();
        }

        private async Task Initialize()
        {
            var budgetsListData = await sender.Send(new LoadBudgetListData.Query());
            Budgets.Clear();
            Budgets.AddRange(
                budgetsListData.OrderBy(bld => bld.Name)
                    .Select(
                        bld => new BudgetListItemViewModel
                        {
                            Id = bld.Id,
                            Name = bld.Name,
                            SpendingLimit = bld.SpendingLimit,
                            CurrentSpending = bld.CurrentSpending
                        }));

            OnPropertyChanged(nameof(BudgetedAmount));
            OnPropertyChanged(nameof(HasBudgets));
        }

        private static async Task GoToAddBudget()
        {
            await Shell.Current.GoToModalAsync(Routes.AddBudgetRoute);
        }

        private async Task EditBudgetAsync(BudgetListItemViewModel? selectedBudget)
        {
            if (selectedBudget == null)
            {
                return;
            }

            await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditBudgetPage(selectedBudget.Id)) { BarBackgroundColor = Colors.Transparent });
        }
    }

}
