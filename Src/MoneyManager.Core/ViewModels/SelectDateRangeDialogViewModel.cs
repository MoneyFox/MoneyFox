using System;
using MoneyManager.Foundation.Messages;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectDateRangeDialogViewModel : BaseViewModel
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
        public MvxCommand DoneCommand => new MvxCommand(Done);

        private void Done()
        {
            MessageHub.Publish(new DateSelectedMessage(this, StartDate, EndDate));
        }
    }
}