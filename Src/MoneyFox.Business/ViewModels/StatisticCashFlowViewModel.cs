using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticCashFlowViewModel : StatisticViewModel, IStatisticCashFlowViewModel
    {
        private readonly CashFlowDataProvider cashFlowDataProvider;

        public StatisticCashFlowViewModel(IMvxMessenger messenger, CashFlowDataProvider cashFlowDataProvider) 
            : base(messenger)
        {
            this.cashFlowDataProvider = cashFlowDataProvider;

            StatisticItems = new MvxObservableCollection<StatisticItem>();
        }

        /// <summary>
        ///     Loads the cashflow with the current start and end date.
        /// </summary>
        protected override void Load()
        {
            LoadCashFlowData();
        }

        /// <summary>
        ///     Contains the Statistic items to display
        /// </summary>
        public MvxObservableCollection<StatisticItem> StatisticItems { get; set; }

        /// <summary>
        ///     Set a custom CashFlowModel with the set Start and Enddate
        /// </summary>
        private void LoadCashFlowData()
        {
            //TODO: Unit Test for order!
            //TODO: Unit Test for selection.
            StatisticItems.Clear();
            StatisticItems.AddRange(cashFlowDataProvider.GetCashFlow(StartDate, EndDate));
        }
    }
}