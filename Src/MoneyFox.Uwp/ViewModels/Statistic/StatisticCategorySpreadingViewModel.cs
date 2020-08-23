using MediatR;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries.GetCategorySpreading;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <summary>
    /// Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private ObservableCollection<StatisticEntry> statisticItems;

        public StatisticCategorySpreadingViewModel(IMediator mediator) : base(mediator)
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

        /// <summary>
        /// Set a custom CategorySpreadingModel with the set Start and End date
        /// </summary>
        protected override async Task LoadAsync()
        {
            StatisticItems = new ObservableCollection<StatisticEntry>(await Mediator.Send(
                new GetCategorySpreadingQuery
                {
                    StartDate = StartDate,
                    EndDate = EndDate
                }));
        }
    }
}
