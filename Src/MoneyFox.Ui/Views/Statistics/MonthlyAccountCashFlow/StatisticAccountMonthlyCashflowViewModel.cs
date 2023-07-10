namespace MoneyFox.Ui.Views.Statistics.MonthlyAccountCashFlow;

using System.Collections.ObjectModel;
using Accounts.AccountModification;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Settings;
using Core.Queries;
using Core.Queries.Statistics;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using SkiaSharp;

internal sealed class StatisticAccountMonthlyCashFlowViewModel : StatisticViewModel
{
    private readonly IMapper mapper;
    private AccountViewModel selectedAccount = null!;

    public StatisticAccountMonthlyCashFlowViewModel(IMediator mediator, IMapper mapper, ISettingsFacade settingsFacade) : base(mediator)
    {
        this.mapper = mapper;
        StartDate = DateTime.Now.AddYears(-1);
    }

    public ObservableCollection<ISeries> Series { get; } = new();

    public List<ICartesianAxis> XAxis { get; } = new() { new Axis() };

    public ObservableCollection<AccountViewModel> Accounts { get; } = new();

    public AccountViewModel SelectedAccount
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
            LoadDataCommand.Execute(null);
        }
    }

    public AsyncRelayCommand InitCommand => new(InitAsync);

    public AsyncRelayCommand LoadDataCommand => new(LoadAsync);

    private async Task InitAsync()
    {
        Accounts.Clear();
        var accounts = mapper.Map<List<AccountViewModel>>(await Mediator.Send(new GetAccountsQuery()));
        accounts.ForEach(Accounts.Add);
        if (Accounts.Any())
        {
            SelectedAccount = Accounts.First();
            await LoadAsync();
        }
    }

    protected override async Task LoadAsync()
    {
        SetXAxis();
        var statisticItems = await Mediator.Send(new GetAccountProgression.Query(accountId: SelectedAccount.Id, startDate: StartDate, endDate: EndDate));
        var columnSeries = new ColumnSeries<decimal>
        {
            Name = string.Empty,
            DataLabelsFormatter = point => $"{point.PrimaryValue}",
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
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new(red: 200, green: 200, blue: 200)),
                SeparatorsAtCenter = false,
                TicksPaint = new SolidColorPaint(new(red: 35, green: 35, blue: 35)),
                TicksAtCenter = true
            });
    }
}
