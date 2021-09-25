using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Domain;
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
        private int numberOfCategoriesToShow = new SettingsFacade(new SettingsAdapter()).CategorySpreadingNumber;
        private PaymentType selectedPaymentType;

        public StatisticCategorySpreadingViewModel(IMediator mediator) : base(mediator)
        {
        }

        public List<PaymentType> PaymentTypes => new List<PaymentType> { PaymentType.Expense, PaymentType.Income };

        public PaymentType SelectedPaymentType
        {
            get => selectedPaymentType;
            set
            {
                if(selectedPaymentType == value)
                {
                    return;
                }
                selectedPaymentType = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Statistic items to display.
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
        ///     Amount of categories to show. All Payments not fitting in here will go to the other category
        /// </summary>
        public int NumberOfCategoriesToShow
        {
            get => numberOfCategoriesToShow;
            set
            {
                if(numberOfCategoriesToShow == value)
                {
                    return;
                }
                numberOfCategoriesToShow = value;
                new SettingsFacade(new SettingsAdapter()).CategorySpreadingNumber = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

        /// <summary>
        /// Set a custom CategorySpreadingModel with the set Start and End date
        /// </summary>
        protected override async Task LoadAsync()
        {
            IEnumerable<StatisticEntry> statisticEntries = await Mediator.Send(new GetCategorySpreadingQuery(StartDate,
                                                                                                                    EndDate,
                                                                                                                    SelectedPaymentType,
                                                                                                                    NumberOfCategoriesToShow));

            statisticEntries.ToList().ForEach(x => x.Label = $"{x.Label} ({x.ValueLabel})");

            StatisticItems = new ObservableCollection<StatisticEntry>(statisticEntries);
        }
    }
}
