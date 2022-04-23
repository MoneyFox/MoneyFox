namespace MoneyFox.Win.ViewModels.Statistics;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Accounts;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Core.ApplicationCore.Queries;
using Core.ApplicationCore.Queries.Statistics;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using SkiaSharp;

/// <summary>
///     Representation of the cash flow view.
/// </summary>
public class StatisticAccountMonthlyCashflowViewModel : StatisticViewModel
{
    private readonly IMapper mapper;
    private AccountViewModel selectedAccount = null!;

    public StatisticAccountMonthlyCashflowViewModel(IMediator mediator, IMapper mapper) : base(mediator)
    {
        this.mapper = mapper;
        StartDate = DateTime.Now.AddYears(-1);
    }

    public ObservableCollection<ISeries> Series { get; } = new();

    public List<ICartesianAxis> XAxis { get; } = new() { new Axis { IsVisible = false } };

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

    public RelayCommand InitCommand => new(async () => await InitAsync());

    public RelayCommand LoadDataCommand => new(async () => await LoadAsync());

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
        var statisticItems = await Mediator.Send(new GetAccountProgressionQuery(accountId: SelectedAccount?.Id ?? 0, startDate: StartDate, endDate: EndDate));
        var columnSeries = new ColumnSeries<decimal>
        {
            TooltipLabelFormatter = point => $"{point.PrimaryValue:C}",
            DataLabelsFormatter = point => $"{point.PrimaryValue:C}",
            DataLabelsPaint = new SolidColorPaint(SKColor.Parse("b4b2b0")),
            Values = statisticItems.Select(x => x.Value)
        };

        Series.Clear();
        Series.Add(columnSeries);
    }
}
