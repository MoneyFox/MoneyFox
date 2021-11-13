using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Application.Common.Extensions;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistics
{
    /// <summary>
    /// Represents the statistic view.
    /// </summary>
    public abstract class StatisticViewModel : ObservableRecipient
    {
        private DateTime startDate;
        private DateTime endDate;

        protected readonly IMediator Mediator;

        /// <summary>
        /// Creates a StatisticViewModel Object and passes the first and last day of the current month     as a start
        /// and end date.
        /// </summary>
        protected StatisticViewModel(IMediator mediator)
            : this(DateTime.Today.GetFirstDayOfMonth(),
                DateTime.Today.GetLastDayOfMonth(),
                mediator)
        {
        }

        /// <summary>
        /// Creates a Statistic ViewModel with custom start and end date
        /// </summary>
        protected StatisticViewModel(
            DateTime startDate,
            DateTime endDate,
            IMediator mediator)
        {
            StartDate = startDate;
            EndDate = endDate;
            Mediator = mediator;
        }
        protected override void OnActivated()
        {
            Messenger.Register<StatisticViewModel, DateSelectedMessage>(this, (r, m) =>
            {
                r.StartDate = m.StartDate;
                r.EndDate = m.EndDate;
                LoadAsync();
            });
        }

        protected override void OnDeactivated()
        {
            Messenger.Unregister<DateSelectedMessage>(this);
        }

        public RelayCommand LoadedCommand => new RelayCommand(async () => await LoadAsync());

        /// <summary>
        /// Start date for a custom statistic
        /// </summary>
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// End date for a custom statistic
        /// </summary>
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// Returns the title for the CategoryViewModel view
        /// </summary>
        public string Title =>
            $"{Strings.StatisticsTimeRangeTitle} {StartDate.ToString("d", CultureInfo.InvariantCulture)} - {EndDate.ToString("d", CultureInfo.InvariantCulture)}";

        protected abstract Task LoadAsync();
    }
}