using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using Microcharts;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Messages;
using MoneyFox.Core.Queries.Categories.GetCategoryById;
using MoneyFox.Core.Queries.Statistics;
using MoneyFox.Core.Queries.Statistics.Queries;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.Statistics;
using MoneyFox.Uwp.Views.Payments;
using MoneyFox.Uwp.Views.Statistics;
using SkiaSharp;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class StatisticCategoryProgressionViewModel : StatisticViewModel
    {
        private BarChart chart = new BarChart();
        private bool hasNoData = true;
        private CategoryViewModel selectedCategory;
        private readonly IMapper mapper;

        public StatisticCategoryProgressionViewModel(
            IMediator mediator,
            IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;
            StartDate = DateTime.Now.AddYears(-1);
        }

        protected override void OnActivated()
        {
            Messenger.Register<StatisticCategoryProgressionViewModel, CategorySelectedMessage>(
                this,
                async (r, m) =>
                {
                    SelectedCategory =
                        mapper.Map<CategoryViewModel>(await Mediator.Send(new GetCategoryByIdQuery(m.CategoryId)));
                    await r.LoadAsync();
                });
        }

        protected override void OnDeactivated()
        {
            Messenger.Unregister<CategorySelectedMessage>(this);
        }

        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if(selectedCategory == value)
                {
                    return;
                }

                selectedCategory = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public RelayCommand GoToSelectCategoryDialogCommand => new RelayCommand(
            async ()
                => await new SelectCategoryDialog { RequestedTheme = ThemeSelectorService.Theme }.ShowAsync());

        protected override async Task LoadAsync()
        {
            if(SelectedCategory == null)
            {
                HasNoData = true;
                return;
            }

            IImmutableList<StatisticEntry> statisticItems =
                await Mediator.Send(new GetCategoryProgressionQuery(SelectedCategory.Id, StartDate, EndDate));

            HasNoData = !statisticItems.Any();

            Chart = new BarChart
            {
                Entries = statisticItems.Select(
                        x => new ChartEntry((float)x.Value)
                        {
                            Label = x.Label,
                            ValueLabel = x.ValueLabel,
                            Color = SKColor.Parse(x.Color),
                            ValueLabelColor = SKColor.Parse(x.Color)
                        })
                    .ToList(),
                BackgroundColor = new SKColor(
                    ChartOptions.BackgroundColor.R,
                    ChartOptions.BackgroundColor.G,
                    ChartOptions.BackgroundColor.B,
                    ChartOptions.BackgroundColor.A),
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = SKTypeface.FromFamilyName(ChartOptions.TypeFace)
            };
        }
    }
}