using System;
using MoneyFox.Business.Messages;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public interface ISelectFilterDialogViewModel : IBaseViewModel
    {
        bool IsClearedFilterActive { get; set; }
        bool IsRecurringFilterActive { get; set; }
        DateTime TimeRangeStart { get; set; }
        DateTime TimeRangeEnd { get; set; }
    }

    public class SelectFilterDialogViewModel : BaseNavigationViewModel, ISelectFilterDialogViewModel
    {
        private readonly IMvxMessenger messenger;

        private bool isClearedFilterActive;
        private bool isRecurringFilterActive;
        private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);
        private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);

        public SelectFilterDialogViewModel(IMvxMessenger messenger,
                                           IMvxLogProvider logProvider,
                                           IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.messenger = messenger;
        }

        /// <summary>
        ///      Indicates wether the filter for only cleared Payments is active or not.
        /// </summary>
        public bool IsClearedFilterActive
        {
            get => isClearedFilterActive;
            set
            {
                if (isClearedFilterActive == value) return;
                isClearedFilterActive = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        /// <summary>
        ///      Indicates wether the filter to only display recurring Payments is active or not.
        /// </summary>
        public bool IsRecurringFilterActive
        {
            get => isRecurringFilterActive;
            set
            {
                if (isRecurringFilterActive == value) return;
                isRecurringFilterActive = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        /// <summary>
        ///     Start of the time range to load payments.
        /// </summary>
        public DateTime TimeRangeStart
        {
            get => timeRangeStart;
            set
            {
                if (timeRangeStart == value) return;
                timeRangeStart = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        /// <summary>
        ///     End of the time range to load payments.
        /// </summary>
        public DateTime TimeRangeEnd
        {
            get => timeRangeEnd;
            set
            {
                if (timeRangeEnd == value) return;
                timeRangeEnd = value;
                RaisePropertyChanged();
                UpdateList();
            }
        }

        private void UpdateList()
        {
            messenger.Publish(new PaymentListFilterChangedMessage(this)
            {
                IsClearedFilterActive = IsClearedFilterActive,
                IsRecurringFilterActive = IsRecurringFilterActive,
                TimeRangeStart = this.TimeRangeStart,
                TimeRangeEnd = this.TimeRangeEnd
            });
        }
    }
}