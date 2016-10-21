using System;
using MoneyFox.Business.Extensions;
using MoneyFox.Foundation.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class SelectDateRangeDialogViewModel : BaseViewModel
    {
        private DateTime startDate;
        private DateTime endDate;

        public SelectDateRangeDialogViewModel()
        {
            StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDate = DateTime.Today.GetLastDayOfMonth();
        }

        /// <summary>
        ///     Startdate for the custom date range
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
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
            get { return endDate; }
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

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        private void Done()
        {
            MessageHub.Publish(new DateSelectedMessage(this, StartDate, EndDate));
        }
    }
}