using System;
using MoneyFox.Business.Extensions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MoneyFox.Business.Messages;

namespace MoneyFox.Business.ViewModels
{
    public class SelectDateRangeDialogViewModel : MvxViewModel
    {
        private readonly IMvxMessenger messenger;

        private DateTime startDate;
        private DateTime endDate;

        public SelectDateRangeDialogViewModel(IMvxMessenger messenger)
        {
            this.messenger = messenger;
            StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDate = DateTime.Today.GetLastDayOfMonth();
        }

        /// <summary>
        ///     Startdate for the custom date range
        /// </summary>
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value; 
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Enddate for the custom date range
        /// </summary>
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value; 
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Selects the dates and notifies observer via the MessageHub
        /// </summary>
        public MvxCommand DoneCommand => new MvxCommand(Done);

        private void Done()
        {
            messenger.Publish(new DateSelectedMessage(this, StartDate, EndDate));
        }
    }
}