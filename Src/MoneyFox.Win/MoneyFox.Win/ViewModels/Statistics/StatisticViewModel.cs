namespace MoneyFox.Win.ViewModels.Statistics;

using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Extensions;
using Core._Pending_.Common.Messages;
using Core.Resources;
using MediatR;

/// <summary>
///     Represents the statistic view.
/// </summary>
public abstract class StatisticViewModel : ObservableRecipient
{
    protected readonly IMediator Mediator;
    private DateTime startDate;
    private DateTime endDate;

    /// <summary>
    ///     Creates a StatisticViewModel Object and passes the first and last day of the current month     as a start
    ///     and end date.
    /// </summary>
    protected StatisticViewModel(IMediator mediator) : this(
        startDate: DateTime.Today.GetFirstDayOfMonth(),
        endDate: DateTime.Today.GetLastDayOfMonth(),
        mediator: mediator) { }

    /// <summary>
    ///     Creates a Statistic ViewModel with custom start and end date
    /// </summary>
    protected StatisticViewModel(DateTime startDate, DateTime endDate, IMediator mediator)
    {
        StartDate = startDate;
        EndDate = endDate;
        Mediator = mediator;
        IsActive = true;
    }

    public RelayCommand LoadedCommand => new(async () => await LoadAsync());

    /// <summary>
    ///     Start date for a custom statistic
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
    ///     End date for a custom statistic
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
    ///     Returns the title for the CategoryViewModel view
    /// </summary>
    public string Title
        => $"{Strings.StatisticsTimeRangeTitle} {StartDate.ToString(format: "d", provider: CultureInfo.InvariantCulture)} - {EndDate.ToString(format: "d", provider: CultureInfo.InvariantCulture)}";

    protected override void OnActivated()
    {
        Messenger.Register<StatisticViewModel, DateSelectedMessage>(
            recipient: this,
            handler: (r, m) =>
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

    protected abstract Task LoadAsync();
}
