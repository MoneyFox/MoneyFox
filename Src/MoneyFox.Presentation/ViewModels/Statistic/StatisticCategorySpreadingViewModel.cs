using MediatR;
using Microcharts;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries.GetCategorySpreading;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <summary>
    /// Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private static readonly string fontFamily = Device.RuntimePlatform == Device.iOS
                                                    ? "Lobster-Regular" : null;
        private readonly SKTypeface typeFaceForIOS12 = SKTypeface.FromFamilyName(fontFamily);

        private DonutChart chart;

        public StatisticCategorySpreadingViewModel(IMediator mediator,
                                                   ISettingsFacade settingsFacade) : base(mediator, settingsFacade)
        {
        }

        /// <summary>
        /// Chart to render.
        /// </summary>
        public DonutChart Chart
        {
            get => chart;
            set
            {
                if(chart == value)
                    return;
                chart = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Set a custom CategorySpreadingModel with the set Start and End date
        /// </summary>
        protected override async Task Load()
        {
            var statisticItems = new ObservableCollection<StatisticEntry>(await Mediator.Send(new GetCategorySpreadingQuery
                                                                                              {
                                                                                                  StartDate = StartDate,
                                                                                                  EndDate = EndDate
                                                                                              }));

            List<Entry> microChartItems = statisticItems
                                         .Select(x => new Entry(x.Value)
                                                      {
                                                          Label = x.Label,
                                                          ValueLabel = x.ValueLabel,
                                                          Color = SKColor.Parse(x.Color)
                                                      })
                                         .ToList();

            Chart = new DonutChart
                    {
                        Entries = microChartItems,
                        BackgroundColor = BackgroundColor,
                        LabelTextSize = 26f,
                        Typeface = typeFaceForIOS12
                    };
        }
    }
}
