namespace MoneyFox.Ui.ViewModels.Statistics;

using System.Collections.ObjectModel;
using AutoMapper;
using Categories;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Queries;
using Core.ApplicationCore.Queries.Statistics;
using Core.Common.Messages;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using SkiaSharp;

internal sealed class StatisticCategoryProgressionViewModel : StatisticViewModel
{
    private readonly IMapper mapper;
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
        private set => SetProperty(field: ref selectedCategory, newValue: value);
    }

    public ObservableCollection<ISeries> Series { get; } = new();

    public List<ICartesianAxis> XAxis { get; } = new() { new Axis() };

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

    public AsyncRelayCommand LoadDataCommand => new(LoadAsync);

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.SelectCategoryRoute));

    protected override void OnActivated()
    {
        Messenger.Register<StatisticCategoryProgressionViewModel, CategorySelectedMessage>(
            recipient: this,
            handler: async (r, m) =>
            {
                SelectedCategory = mapper.Map<CategoryViewModel>(await Mediator.Send(new GetCategoryByIdQuery(m.Value.CategoryId)));
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

        var statisticItems = await Mediator.Send(new GetCategoryProgressionQuery(categoryId: SelectedCategory.Id, startDate: StartDate, endDate: EndDate));
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
