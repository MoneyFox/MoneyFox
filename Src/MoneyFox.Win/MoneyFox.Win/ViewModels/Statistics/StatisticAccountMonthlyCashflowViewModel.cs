using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MediatR;
using MoneyFox.Core.Queries.Accounts.GetAccounts;
using MoneyFox.Core.Queries.Statistics;
using MoneyFox.Core.Queries.Statistics.Queries;
using MoneyFox.Win.ViewModels.Accounts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Win.ViewModels.Statistics
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class StatisticAccountMonthlyCashflowViewModel : StatisticViewModel
    {
        private AccountViewModel selectedAccount = null!;
        private readonly IMapper mapper;

        public StatisticAccountMonthlyCashflowViewModel(IMediator mediator,
            IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;

            StartDate = DateTime.Now.AddYears(-1);
        }

        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

        public List<ICartesianAxis> XAxis { get; } = new List<ICartesianAxis>
        {
            new Axis { IsVisible = false }
        };

        public ObservableCollection<AccountViewModel> Accounts { get; } = new ObservableCollection<AccountViewModel>();

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                if(selectedAccount == value)
                {
                    return;
                }

                selectedAccount = value;
                OnPropertyChanged();
                LoadDataCommand.Execute(null);
            }
        }

        public RelayCommand InitCommand => new RelayCommand(async () => await InitAsync());

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

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