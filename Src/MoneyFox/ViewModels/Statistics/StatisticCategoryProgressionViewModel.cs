namespace MoneyFox.ViewModels.Statistics
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Categories;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using Core.Queries;
    using Core.Queries.Statistics;
    using Extensions;
    using MediatR;
    using Microcharts;
    using SkiaSharp;
    using Views.Statistics;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class StatisticCategoryProgressionViewModel : StatisticViewModel
    {
        private readonly IMapper mapper;
        private BarChart chart = new BarChart();
        private bool hasNoData = true;
        private CategoryViewModel? selectedCategory;

        public StatisticCategoryProgressionViewModel(IMediator mediator, IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;
            StartDate = DateTime.Now.AddYears(-1);
        }

        public CategoryViewModel? SelectedCategory
        {
            get => selectedCategory;

            set
            {
                if (selectedCategory == value)
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
                if (chart == value)
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
                if (hasNoData == value)
                {
                    return;
                }

                hasNoData = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

        public RelayCommand GoToSelectCategoryDialogCommand
            => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.SelectCategoryRoute));

        protected override void OnActivated()
        {
            Messenger.Register<StatisticCategoryProgressionViewModel, CategorySelectedMessage>(
                recipient: this,
                handler: async (r, m) =>
                {
                    SelectedCategory = mapper.Map<CategoryViewModel>(await Mediator.Send(new GetCategoryByIdQuery(m.CategoryId)));
                    await r.LoadAsync();
                });
        }

        protected override void OnDeactivated()
        {
            Messenger.Unregister<CategorySelectedMessage>(this);
        }

        protected override async Task LoadAsync()
        {
            if (SelectedCategory == null)
            {
                HasNoData = true;

                return;
            }

            var statisticItems = await Mediator.Send(
                new GetCategoryProgressionQuery(categoryId: SelectedCategory?.Id ?? 0, startDate: StartDate, endDate: EndDate));

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
                BackgroundColor = new SKColor(ChartOptions.BackgroundColor.ToUInt()),
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = SKTypeface.FromFamilyName(ChartOptions.TypeFace)
            };
        }
    }

}
