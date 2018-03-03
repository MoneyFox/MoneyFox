using System.Threading.Tasks;
using Microcharts;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class StatisticCashFlowViewModel : StatisticViewModel, IStatisticCashFlowViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly CashFlowDataProvider cashFlowDataProvider;
        private BarChart chart;

        public StatisticCashFlowViewModel(IMvxMessenger messenger, CashFlowDataProvider cashFlowDataProvider, ISettingsManager settingsManager) 
            : base(messenger, settingsManager)
        {
            this.cashFlowDataProvider = cashFlowDataProvider;
            this.settingsManager = settingsManager;

            Chart = new BarChart();
        }
        
        /// <summary>
        ///     Chart to render.
        /// </summary>
        public BarChart Chart
        {
            get => chart;
            set
            {
                if (chart == value) return;
                chart = value;
                RaisePropertyChanged();
            }
        }

        public override async Task Initialize()
        {
            await Load();
        }

        protected override async Task Load()
        {
            var entries = await cashFlowDataProvider.GetCashFlow(StartDate, EndDate);
            Chart = new BarChart
            {
                Entries = entries,
                BackgroundColor = BackgroundColor,
                Margin = 12
            };
        }
    }
}