namespace MoneyFox.Ui.Views.Statistics;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Extensions;

public sealed class SelectDateRangeDialogViewModel : ObservableRecipient
{
    private DateTime endDate;
    private DateTime startDate;

    public SelectDateRangeDialogViewModel()
    {
        StartDate = new(
            year: DateTime.Today.Year,
            month: DateTime.Today.Month,
            day: 1,
            hour: 0,
            minute: 0,
            second: 0,
            kind: DateTimeKind.Local);

        EndDate = DateTime.Today.GetLastDayOfMonth();
    }

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

    private void Done()
    {
        Messenger.Send(new DateSelectedMessage(startDate: StartDate, endDate: EndDate));
    }
}
