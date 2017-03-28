using System.Collections.ObjectModel;
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
        ///     Used by Android
        /// </summary>
        public CashFlow CashFlow { get; set; }

        /// <summary>
        ///     Used by Windows
        /// </summary>
        public MvxObservableCollection<StatisticItem> StatisticItems { get; set; }

        /// <summary>
        ///     Set a custom CashFlowModel with the set Start and Enddate
        /// </summary>
        private void LoadCashFlowData()
        {
            var cashFlow = cashFlowDataProvider.GetCashFlow(StartDate, EndDate);

            StatisticItems.Clear();
            StatisticItems.Add(new StatisticItem {Label = cashFlow.Income.Label, Value = cashFlow.Income.Value});
            StatisticItems.Add(new StatisticItem { Label = cashFlow.Expense.Label, Value = cashFlow.Expense.Value });
            StatisticItems.Add(new StatisticItem { Label = cashFlow.Revenue.Label, Value = cashFlow.Revenue.Value });
        }
    }
}