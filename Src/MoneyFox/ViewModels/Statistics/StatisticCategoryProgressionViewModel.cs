namespace MoneyFox.ViewModels.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Categories;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.ApplicationCore.Queries;
    using Core.ApplicationCore.Queries.Statistics;
    using Core.Common.Messages;
    using Extensions;
    using LiveChartsCore;
    using LiveChartsCore.Kernel.Sketches;
    using LiveChartsCore.SkiaSharpView;
    using LiveChartsCore.SkiaSharpView.Painting;
    using MediatR;
    using SkiaSharp;
    using Xamarin.Forms;

    public class StatisticCategoryProgressionViewModel : StatisticViewModel
    {
        private bool hasNoData = true;
        private CategoryViewModel? selectedCategory;

        private readonly IMapper mapper;

        public StatisticCategoryProgressionViewModel(IMediator mediator, IMapper mapper) : base(mediator)
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
                    SelectedCategory = mapper.Map<CategoryViewModel>(await Mediator.Send(new GetCategoryByIdQuery(m.Value.CategoryId)));
                    await r.LoadAsync();
                });
        }

        protected override void OnDeactivated()
        {
            Messenger.Unregister<CategorySelectedMessage>(this);
        }

        public CategoryViewModel? SelectedCategory
        {
            get => selectedCategory;
            private set => SetProperty(ref selectedCategory, value);
        }

        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

        public List<ICartesianAxis> XAxis { get; } = new List<ICartesianAxis>
        {
            new Axis()
        };

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

        public AsyncRelayCommand LoadDataCommand => new AsyncRelayCommand(LoadAsync);

        public AsyncRelayCommand GoToSelectCategoryDialogCommand => new AsyncRelayCommand(
            async () =>
                await Shell.Current.GoToModalAsync(ViewModelLocator.SelectCategoryRoute));

        protected override async Task LoadAsync()
        {
            if (SelectedCategory == null)
            {
                HasNoData = true;
                return;
            }

            IImmutableList<StatisticEntry> statisticItems =
                await Mediator.Send(new GetCategoryProgressionQuery(SelectedCategory.Id, StartDate, EndDate));

            HasNoData = !statisticItems.Any();

            var columnSeries = new ColumnSeries<decimal>
            {
                Name = SelectedCategory.Name,
                TooltipLabelFormatter = point => $"{point.PrimaryValue:C}",
                DataLabelsFormatter = point => $"{point.PrimaryValue:C}",
                DataLabelsPaint = new SolidColorPaint(SKColor.Parse("b4b2b0")),
                Values = statisticItems.Select(x => x.Value),
                Stroke = new SolidColorPaint(SKColors.DarkRed)
            };

            Series.Clear();
            Series.Add(columnSeries);
        }
    }
}
