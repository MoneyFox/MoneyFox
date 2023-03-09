namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using System.Collections.ObjectModel;
using Categories.CategorySelection;
using Common.Extensions;
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

internal sealed class StatisticCategoryProgressionViewModel : StatisticViewModel, IQueryAttributable
{
    private bool hasNoData = true;

    public StatisticCategoryProgressionViewModel(IMediator mediator, CategorySelectionViewModel categorySelectionViewModel) : base(mediator)
    {
        CategorySelectionViewModel = categorySelectionViewModel;
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

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(async () => await Shell.Current.GoToModalAsync(Routes.SelectCategoryRoute));

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(key: SelectCategoryViewModel.SELECTED_CATEGORY_ID_PARAM, value: out var selectedCategoryIdParam))
        {
            var selectedCategoryId = Convert.ToInt32(selectedCategoryIdParam);
            var category = Mediator.Send(new GetCategoryByIdQuery(selectedCategoryId)).GetAwaiter().GetResult();
            CategorySelectionViewModel.SelectedCategory = new() { Id = category.Id, Name = category.Name, RequireNote = category.RequireNote };
            LoadAsync().GetAwaiter().GetResult();
        }
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
            new GetCategoryProgressionQuery(categoryId: CategorySelectionViewModel.SelectedCategory.Id, startDate: StartDate, endDate: EndDate));

        HasNoData = !statisticItems.Any();
        var columnSeries = new ColumnSeries<decimal>
        {
            Name = CategorySelectionViewModel.SelectedCategory.Name,
            TooltipLabelFormatter = point => $"{point.PrimaryValue:C}",
            DataLabelsFormatter = point => $"{point.PrimaryValue:C}",
            DataLabelsPaint = new SolidColorPaint(SKColor.Parse("b4b2b0")),
            Values = statisticItems.Select(x => x.Value),
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
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new(red: 200, green: 200, blue: 200)),
                SeparatorsAtCenter = false,
                TicksPaint = new SolidColorPaint(new(red: 35, green: 35, blue: 35)),
                TicksAtCenter = true
            });
    }
}
