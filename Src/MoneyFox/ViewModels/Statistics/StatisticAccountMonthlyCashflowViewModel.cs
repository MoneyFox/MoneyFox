namespace MoneyFox.ViewModels.Statistics
{
    using Accounts;
    using AutoMapper;
    using CommunityToolkit.Mvvm.Input;
    using Core.Queries.Statistics;
    using MediatR;
    using Microcharts;
    using SkiaSharp;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Queries;
    using Views.Statistics;
    using Xamarin.Essentials;

    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class StatisticAccountMonthlyCashflowViewModel : StatisticViewModel
    {
        private readonly IMapper mapper;
        private BarChart chart = new BarChart();
        private bool hasNoData;
        private AccountViewModel selectedAccount = null!;

        public StatisticAccountMonthlyCashflowViewModel(
            IMediator mediator,
            IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;

            StartDate = DateTime.Now.AddYears(-1);
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public BarChart Chart
        {
            get => chart;
            set
            {
                if(chart == value)
                {
                    return;
                }

                chart = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public bool HasNoData
        {
            get => hasNoData;
            set
            {
                if(hasNoData == value)
                {
                    return;
                }

                hasNoData = value;
                OnPropertyChanged();
            }
        }

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

            HasNoData = !statisticItems.Any();

            Chart = new BarChart
            {
                Entries = statisticItems.Select(
                        x => new ChartEntry((float)x.Value)
                        {
                            Label = x.Label,
                            ValueLabel = x.ValueLabel,
                            Color = SKColor.Parse(x.Color),
                            ValueLabelColor = SKColor.Parse(x.Color)
                        })
                    .ToList(),
                BackgroundColor = new SKColor(ChartOptions.BackgroundColor.ToUInt()),
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = SKTypeface.FromFamilyName(ChartOptions.TypeFace)
            };
        }
    }
}