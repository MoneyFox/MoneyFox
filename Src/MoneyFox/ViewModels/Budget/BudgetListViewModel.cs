namespace MoneyFox.ViewModels.Budget
{

    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using JetBrains.Annotations;
    using MediatR;

    public sealed class BudgetListViewModel : ObservableRecipient
    {
        private readonly IMediator mediator;

        public BudgetListViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private ObservableCollection<BudgetViewModel> budgets = new ObservableCollection<BudgetViewModel>();
        public ObservableCollection<BudgetViewModel> Budgets
        {
            get => budgets;

            private set
            {
                budgets = value;
                OnPropertyChanged();
            }
        }

        public AsyncRelayCommand GoToAddBudgetCommand => new AsyncRelayCommand(GoToAddBudget);
        public AsyncRelayCommand EditBudgetCommand => new AsyncRelayCommand(EditBudget);

        private Task GoToAddBudget()
        {
            throw new System.NotImplementedException();
        }

        private Task EditBudget()
        {
            throw new System.NotImplementedException();
        }
    }

}
