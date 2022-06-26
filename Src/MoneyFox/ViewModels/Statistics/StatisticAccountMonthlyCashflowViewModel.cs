namespace MoneyFox.ViewModels.Statistics
{

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

    internal sealed class StatisticAccountMonthlyCashFlowViewModel : StatisticViewModel
    {
        private readonly IMapper mapper;
        private AccountViewModel selectedAccount = null!;

        public StatisticAccountMonthlyCashFlowViewModel(IMediator mediator, IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;
            StartDate = DateTime.Now.AddYears(-1);
        }

        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

        public List<ICartesianAxis> XAxis { get; } = new List<ICartesianAxis> { new Axis() };

        public ObservableCollection<AccountViewModel> Accounts { get; } = new ObservableCollection<AccountViewModel>();

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

        public AsyncRelayCommand InitCommand => new AsyncRelayCommand(InitAsync);

        public AsyncRelayCommand LoadDataCommand => new AsyncRelayCommand(LoadAsync);

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
            var statisticItems = await Mediator.Send(
                new GetAccountProgressionQuery(accountId: SelectedAccount?.Id ?? 0, startDate: StartDate, endDate: EndDate));

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
