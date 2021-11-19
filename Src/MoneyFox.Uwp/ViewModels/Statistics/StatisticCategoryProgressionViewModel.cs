#nullable enable
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JetBrains.Annotations;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using Microcharts;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Categories;
using MoneyFox.Uwp.Views.Payments;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistics
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    [UsedImplicitly]
    public class StatisticCategoryProgressionViewModel : StatisticViewModel
    {
        private bool hasNoData = true;
        private CategoryViewModel? selectedCategory;
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
                    SelectedCategory = mapper.Map<CategoryViewModel>(await Mediator.Send(new GetCategoryByIdQuery(m.CategoryId)));
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
            new Axis { IsVisible = false }
        };

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public bool HasNoData
        {
            get => hasNoData;
            private set => SetProperty(ref hasNoData, value);
        }

        public AsyncRelayCommand GoToSelectCategoryDialogCommand => new AsyncRelayCommand(
            async ()
                => await new SelectCategoryDialog {RequestedTheme = ThemeSelectorService.Theme}.ShowAsync());

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

            var columnSeries = new ColumnSeries<decimal>
            {
                Name = SelectedCategory.Name,
                TooltipLabelFormatter = point => $"{point.PrimaryValue:C}",
                DataLabelsFormatter = point => $"{point.PrimaryValue:C}",
                DataLabelsPaint = new SolidColorPaint(SKColor.Parse("b4b2b0")),
                Values = statisticItems.Select(x => x.Value), Stroke = new SolidColorPaint(SKColors.DarkRed)
            };

            Series.Clear();
            Series.Add(columnSeries);
        }
    }
}