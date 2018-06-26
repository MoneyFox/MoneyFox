using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microcharts;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using SkiaSharp;

namespace MoneyFox.Business.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class 
        StatisticCashFlowViewModel : StatisticViewModel, IStatisticCashFlowViewModel
    {
        private readonly CashFlowDataProvider cashFlowDataProvider;
        private BarChart chart;

        public StatisticCashFlowViewModel(IMvxMessenger messenger, CashFlowDataProvider cashFlowDataProvider, ISettingsManager settingsManager) 
            : base(messenger, settingsManager)
        {
            this.cashFlowDataProvider = cashFlowDataProvider;

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

        /// <summary>
        ///     Statistic items to display.
        /// </summary>
        public ObservableCollection<StatisticEntry> StatisticItems { get; set; }

        public override async Task Initialize()
        {
            await Load();
        }

        protected override async Task Load()
        {
            StatisticItems = new MvxObservableCollection<StatisticEntry>(
                await cashFlowDataProvider.GetCashFlow(StartDate, EndDate));

            Chart = new BarChart
            {
                Entries = StatisticItems.Select(x => new Entry(x.Value)
                                        {
                                            Label = x.Label,
                                            ValueLabel = x.ValueLabel,
                                            Color = SKColor.Parse(x.Color)
                                        })
                                        .ToList(),
                BackgroundColor = BackgroundColor,
                Margin = 20,
                LabelTextSize = 26f
            };
        }
    }
}