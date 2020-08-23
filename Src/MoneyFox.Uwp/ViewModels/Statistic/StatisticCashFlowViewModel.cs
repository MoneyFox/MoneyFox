using MediatR;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <summary>
    /// Representation of the cash flow view.
    /// </summary>
    public class StatisticCashFlowViewModel : StatisticViewModel, IStatisticCashFlowViewModel
    {
        private ObservableCollection<StatisticEntry> statisticItems = new ObservableCollection<StatisticEntry>();

        public StatisticCashFlowViewModel(IMediator mediator)
            : base(mediator)
        {
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

        protected override async Task LoadAsync()
        {
            StatisticItems = new ObservableCollection<StatisticEntry>(
                await Mediator.Send(new GetCashFlowQuery
                {
                    EndDate = EndDate,
                    StartDate = StartDate
                }));
        }
    }
}
