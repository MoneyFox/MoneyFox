using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <summary>
    /// Representation of the cash flow view.
    /// </summary>
    public class StatistcAccountMonthlyCashflowViewModel : StatisticViewModel, IStatisticCashFlowViewModel
    {
        private AccountViewModel selectedAccount = null!;
        private ObservableCollection<StatisticEntry> statisticItems = new ObservableCollection<StatisticEntry>();

        public StatistcAccountMonthlyCashflowViewModel(IMediator mediator, IDialogService dialogService)
            : base(mediator, dialogService)
        {
        }

        public ObservableCollection<AccountViewModel> Accounts { get; private set; } = new ObservableCollection<AccountViewModel>();

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
                LoadDataCommand.Execute(null);
            }
        }

        /// <summary>
        /// Statistic items to display.
        /// </summary>
        public ObservableCollection<StatisticEntry> StatisticItems
        {
            get => statisticItems;
            private set
            {
                if(statisticItems == value)
                {
                    return;
                }

                statisticItems = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

        protected override async Task LoadAsync()
        {
            StatisticItems = new ObservableCollection<StatisticEntry>(
                await Mediator.Send(new GetAccountProgressionQuery(SelectedAccount.Id, EndDate, StartDate)));
        }
    }
}
