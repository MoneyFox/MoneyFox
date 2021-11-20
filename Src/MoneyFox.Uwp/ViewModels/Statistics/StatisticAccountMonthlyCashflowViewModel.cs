using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Uwp.ViewModels.Accounts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistics
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    [UsedImplicitly]
    public class StatisticAccountMonthlyCashFlowViewModel : StatisticViewModel
    {
        private AccountViewModel selectedAccount = null!;
        private readonly IMapper mapper;

        public StatisticAccountMonthlyCashFlowViewModel(IMediator mediator,
            IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;

            StartDate = DateTime.Now.AddYears(-1);
        }

        public AsyncRelayCommand InitCommand => new AsyncRelayCommand(async () => await InitAsync());

        public AsyncRelayCommand LoadDataCommand => new AsyncRelayCommand(async () => await LoadAsync());

        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

        public List<ICartesianAxis> XAxis { get; } = new List<ICartesianAxis>
        {
            new Axis { IsVisible = false }
        };

        public ObservableCollection<AccountViewModel> Accounts { get; } = new ObservableCollection<AccountViewModel>();

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set => SetProperty(ref selectedAccount, value);
        }

        private async Task InitAsync()
        {
            Accounts.Clear();
            var accounts = mapper.Map<List<AccountViewModel>>(await Mediator.Send(new GetAccountsQuery()));
            accounts.ForEach(Accounts.Add);

            if(Accounts.Any())
            {
                SelectedAccount = Accounts.First();
                await LoadAsync();
            }
        }

        protected override async Task LoadAsync()
        {
            List<StatisticEntry> statisticItems =
                await Mediator.Send(new GetAccountProgressionQuery(SelectedAccount?.Id ?? 0, StartDate, EndDate));

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
}