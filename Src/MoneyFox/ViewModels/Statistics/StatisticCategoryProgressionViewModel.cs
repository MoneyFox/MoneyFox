using AutoMapper;
using GalaSoft.MvvmLight.Command;
using MediatR;
using Microcharts;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Extensions;
using MoneyFox.Messages;
using MoneyFox.Ui.Shared.ViewModels.Categories;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using SkiaSharp;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Statistics
{
    /// <summary>
    /// Representation of the cash flow view.
    /// </summary>
    public class StatisticCategoryProgressionViewModel : StatisticViewModel
    {
        private BarChart chart = new BarChart();
        private bool hasNoData = true;
        private CategoryViewModel? selectedCategory;
        private readonly IMapper mapper;

        public StatisticCategoryProgressionViewModel(IMediator mediator,
                                                     IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;

            StartDate = DateTime.Now.AddYears(-1);

            MessengerInstance.Register<CategorySelectedMessage>(this, async message => await ReceiveMessageAsync(message));
        }

        public CategoryViewModel? SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if(selectedCategory == value)
                {
                    return;
                }

                selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public BarChart Chart
        {
            get => chart;
            set
            {
                if(chart == value)
                {
                    return;
                }

                chart = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public bool HasNoData
        {
            get => hasNoData;
            set
            {
                if(hasNoData == value)
                {
                    return;
                }

                hasNoData = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

        public RelayCommand GoToSelectCategoryDialogCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.SelectCategoryRoute));

        public RelayCommand ResetCategoryCommand => new RelayCommand(() => SelectedCategory = null);


        private async Task ReceiveMessageAsync(CategorySelectedMessage message)
        {
            if(message == null)
            {
                return;
            }

            SelectedCategory = mapper.Map<CategoryViewModel>(await Mediator.Send(new GetCategoryByIdQuery(message.CategoryId)));
            await LoadAsync();
        }


        protected override async Task LoadAsync()
        {
            if(SelectedCategory == null)
            {
                HasNoData = true;
                return;
            }

            IImmutableList<StatisticEntry> statisticItems = await Mediator.Send(new GetCategoryProgressionQuery(SelectedCategory?.Id ?? 0, StartDate, EndDate));

            HasNoData = !statisticItems.Any();

            Chart = new BarChart
            {
                Entries = statisticItems.Select(x => new ChartEntry((float)x.Value)
                {
                    Label = x.Label,
                    ValueLabel = x.ValueLabel,
                    Color = SKColor.Parse(x.Color),
                    ValueLabelColor = SKColor.Parse(x.Color)
                }).ToList(),
                BackgroundColor = ChartOptions.BackgroundColor,
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = ChartOptions.TypeFace
            };
        }
    }
}
