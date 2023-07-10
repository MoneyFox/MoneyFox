namespace MoneyFox.Ui.Views.Statistics;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Extensions;

internal sealed class SelectDateRangeDialogViewModel : BasePageViewModel
{
    private DateTime endDate;
    private DateTime startDate;

    public SelectDateRangeDialogViewModel()
    {
        StartDate = new(year: DateTime.Today.Year, month: DateTime.Today.Month, day: 1);
        EndDate = DateTime.Today.GetLastDayOfMonth();
    }

    /// <summary>
    ///     Start Date for the custom date range
    /// </summary>
    public DateTime StartDate
    {
        get => startDate;

        set
        {
            startDate = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsDateRangeValid));
        }
    }

    /// <summary>
    ///     End Date for the custom date range
    /// </summary>
    public DateTime EndDate
    {
        get => endDate;

        set
        {
            endDate = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsDateRangeValid));
        }
    }

    public bool IsDateRangeValid => StartDate <= EndDate;

    public RelayCommand DoneCommand => new(Done);

    public void Initialize(DateSelectedMessage message)
    {
        StartDate = message.StartDate;
        EndDate = message.EndDate;
    }

    private void Done()
    {
        Messenger.Send(new DateSelectedMessage(startDate: StartDate, endDate: EndDate));
    }
}
