using System.Threading.Tasks;
using Microcharts;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Plugins.Messenger;
using SkiaSharp;

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
            : base(messenger)
        {
            this.cashFlowDataProvider = cashFlowDataProvider;
            this.settingsManager = settingsManager;

            Chart = new BarChart();
        }

        public override async Task Initialize()
        {
            await Load();
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

        protected override async Task Load()
        {
            var entries = await cashFlowDataProvider.GetCashFlow(StartDate, EndDate);
            Chart = new BarChart
            {
                Entries = entries,
                BackgroundColor = settingsManager.Theme == AppTheme.Dark
                    ? new SKColor(0, 0, 0)
                    : new SKColor(255, 255, 255) 
            };
        }
    }
}