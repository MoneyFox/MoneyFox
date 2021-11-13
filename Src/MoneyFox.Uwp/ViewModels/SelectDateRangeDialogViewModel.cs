using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoneyFox.Application.Common.Extensions;
using MoneyFox.Application.Common.Messages;
using System;

#nullable enable
namespace MoneyFox.Uwp.ViewModels
{
    public class SelectDateRangeDialogViewModel : ObservableRecipient
    {
        private DateTime startDate;
        private DateTime endDate;

        public SelectDateRangeDialogViewModel()
        {
            StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDate = DateTime.Today.GetLastDayOfMonth();
        }

        /// <summary>
        /// Start Date for the custom date range
        /// </summary>
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// End Date for the custom date range
        /// </summary>
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selects the dates and notifies observer via the MessageHub
        /// </summary>
        public RelayCommand DoneCommand => new RelayCommand(Done);

        private void Done() => Messenger.Send(new DateSelectedMessage(this, StartDate, EndDate));
    }
}