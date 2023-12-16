namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using System.Collections.ObjectModel;
using Categories.CategorySelection;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Controls.CategorySelection;
using Core.Queries;
using Core.Queries.Statistics;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using SkiaSharp;

internal sealed class StatisticCategoryProgressionViewModel : StatisticViewModel
{
    private readonly INavigationService navigationService;
    private bool hasNoData = true;

    public StatisticCategoryProgressionViewModel(
        IMediator mediator,
        CategorySelectionViewModel categorySelectionViewModel,
        INavigationService navigationService) : base(mediator)
    {
        CategorySelectionViewModel = categorySelectionViewModel;
        this.navigationService = navigationService;
        StartDate = DateTime.Now.AddYears(-1);
    }

    public CategorySelectionViewModel CategorySelectionViewModel { get; }

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

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(() => navigationService.GoTo<SelectCategoryViewModel>());

    public override async Task OnNavigatedBackAsync(object? parameter)
    {
        var selectedCategoryId = Convert.ToInt32(parameter);
        var category = await Mediator.Send(new GetCategoryByIdQuery(selectedCategoryId));
        CategorySelectionViewModel.SelectedCategory = new() { Id = category.Id, Name = category.Name, RequireNote = category.RequireNote };
        await LoadAsync();
    }

    protected override async Task LoadAsync()
    {
        if (CategorySelectionViewModel.SelectedCategory == null)
        {
            HasNoData = true;

            return;
        }

        SetXAxis();
        var statisticItems = await Mediator.Send(
            new GetCategoryProgression.Query(
                categoryId: CategorySelectionViewModel.SelectedCategory.Id,
                startDate: DateOnly.FromDateTime(StartDate),
                endDate: DateOnly.FromDateTime(EndDate)));

        HasNoData = !statisticItems.Any();
        var columnSeries = new ColumnSeries<decimal>
        {
            Name = CategorySelectionViewModel.SelectedCategory.Name,
            DataLabelsFormatter = point => $"{-point.Coordinate.PrimaryValue:C}",
            DataLabelsPaint = new SolidColorPaint(SKColor.Parse("b4b2b0")),
            Values = statisticItems.Select(x => -x.Value),
            Stroke = new SolidColorPaint(SKColors.DarkRed)
        };

        Series.Clear();
        Series.Add(columnSeries);
    }

    private void SetXAxis()
    {
        var monthLabels = new List<string>();
        var startDate = StartDate;
        while (startDate < EndDate)
        {
            monthLabels.Add(startDate.ToString("MMM"));
            startDate = startDate.AddMonths(1);
        }

        XAxis.Clear();
        XAxis.Add(
            new Axis
            {
                Labels = monthLabels,
                LabelsRotation = 45,
                SeparatorsPaint = new SolidColorPaint(new(red: 200, green: 200, blue: 200)),
                SeparatorsAtCenter = false,
                TicksPaint = new SolidColorPaint(new(red: 35, green: 35, blue: 35)),
                TicksAtCenter = true
            });
    }
}
