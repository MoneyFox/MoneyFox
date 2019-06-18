using System;
using MoneyFox.BusinessLogic.Extensions;
using MoneyFox.ServiceLayer.Messages;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.Presentation.ViewModels
{
    public interface ISelectDateRangeDialogViewModel : IBaseViewModel
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }

        MvxCommand DoneCommand { get; set; }
    }

    public class SelectDateRangeDialogViewModel : BaseNavigationViewModel
    {
        private readonly IMvxMessenger messenger;

        private DateTime startDate;
        private DateTime endDate;

        public SelectDateRangeDialogViewModel(IMvxMessenger messenger,
                                              IMvxLogProvider logProvider,
                                              IMvxNavigationService navigationService) : base(logProvider, navigationService)
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