using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <summary>
    /// Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private ObservableCollection<StatisticEntry> statisticItems = new ObservableCollection<StatisticEntry>();

        public StatisticCategorySpreadingViewModel(IMediator mediator, IDialogService dialogService) : base(mediator, dialogService)
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
            IEnumerable<StatisticEntry> statisticEntries = await Mediator.Send(
                new GetCategorySpreadingQuery
                {
                    StartDate = StartDate,
                    EndDate = EndDate
                });

            statisticEntries.ToList().ForEach(x => x.Label = $"{x.Label} ({x.ValueLabel})");

            StatisticItems = new ObservableCollection<StatisticEntry>(statisticEntries);
        }
    }
}
