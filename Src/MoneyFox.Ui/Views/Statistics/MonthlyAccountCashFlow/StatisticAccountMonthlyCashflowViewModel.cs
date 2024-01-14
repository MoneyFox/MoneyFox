namespace MoneyFox.Ui.Views.Statistics.MonthlyAccountCashFlow;

using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using Core.Queries;
using Core.Queries.Statistics;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using Resources.Strings;
using SkiaSharp;

internal sealed class StatisticAccountMonthlyCashFlowViewModel : StatisticViewModel
{
    private static readonly CashFlowAccountViewModel allAccountsPlaceHolder = new(AccountId: -1, Name: Translations.AllAccounts);
    private CashFlowAccountViewModel? selectedAccount;

    public StatisticAccountMonthlyCashFlowViewModel(IMediator mediator, IPopupService popupService) : base(mediator: mediator, popupService: popupService)
    {
        StartDate = DateTime.Now.AddYears(-1);
    }

    public ObservableCollection<ISeries> Series { get; } = [];

    public List<ICartesianAxis> XAxis { get; } = [new Axis()];

    public ObservableCollection<CashFlowAccountViewModel> Accounts { get; } = new();

    public CashFlowAccountViewModel? SelectedAccount
    {
        get => selectedAccount;

        set
        {
            if (selectedAccount == value)
            {
                return;
            }

            selectedAccount = value;
            OnPropertyChanged();
            LoadAsync().GetAwaiter().GetResult();
        }
    }

    public override async Task OnNavigatedAsync(object? parameter)
    {
        var accountsData = await Mediator.Send(new GetAccountsQuery());
        Accounts.Add(allAccountsPlaceHolder);
        accountsData.Select(a => new CashFlowAccountViewModel(AccountId: a.Id, Name: a.Name)).ToList().ForEach(Accounts.Add);
        SelectedAccount = allAccountsPlaceHolder;
    }

    protected override async Task LoadAsync()
    {
        if (SelectedAccount is null)
        {
            return;
        }

        SetXAxis();
        var statisticItems = await Mediator.Send(new GetAccountProgression.Query(accountId: SelectedAccount.AccountId, startDate: StartDate, endDate: EndDate));
        var columnSeries = new ColumnSeries<decimal>
        {
            Name = string.Empty,
            DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue}",
            DataLabelsPaint = new SolidColorPaint(SKColor.Parse("b4b2b0")),
            Values = statisticItems.Select(x => x.Value)
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

    public record CashFlowAccountViewModel(int AccountId, string Name);
}
