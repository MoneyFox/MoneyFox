namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using System.Collections.ObjectModel;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using Core.Queries.Statistics;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using Messages;
using MoneyFox.Ui.Controls.CategorySelection;
using SkiaSharp;

internal sealed class StatisticCategoryProgressionViewModel : StatisticViewModel, IRecipient<CategorySelectedMessage>
{
    private bool hasNoData = true;
    private SelectedCategoryViewModel? selectedCategory;

    public StatisticCategoryProgressionViewModel(IMediator mediator) : base(mediator)
    {
        StartDate = DateTime.Now.AddYears(-1);
    }

    public SelectedCategoryViewModel? SelectedCategory
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

    public void Receive(CategorySelectedMessage message)
    {
        var category = Mediator.Send(new GetCategoryByIdQuery(message.Value)).GetAwaiter().GetResult();
        SelectedCategory = new() { Id = category.Id, Name = category.Name };
        LoadAsync().GetAwaiter().GetResult();
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
