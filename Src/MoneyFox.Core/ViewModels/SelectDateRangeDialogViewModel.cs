using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Core.Messages;
using PropertyChanged;

namespace MoneyFox.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectDateRangeDialogViewModel : ViewModelBase
    {
        public SelectDateRangeDialogViewModel()
        {
            StartDate = DateTime.Today.Date.AddDays(-1);
            EndDate = DateTime.Today;
        }

        /// <summary>
        ///     Startdate for the custom date range
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Enddate for the custom date range
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     Selects the dates and notifies observer via the MessageHub
        /// </summary>
        public RelayCommand DoneCommand => new RelayCommand(Done);

        private void Done()
        {
            MessengerInstance.Send(new DateSelectedMessage(StartDate, EndDate));
        }
    }
}