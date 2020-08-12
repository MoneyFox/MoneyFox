using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.Messages;
using System;

namespace MoneyFox.ViewModels.Dialogs
{
    public interface ISelectFilterDialogViewModel
    {
        bool IsClearedFilterActive { get; set; }

        bool IsRecurringFilterActive { get; set; }

        DateTime TimeRangeStart { get; set; }

        DateTime TimeRangeEnd { get; set; }
    }

    public class SelectFilterDialogViewModel : ViewModelBase, ISelectFilterDialogViewModel
    {
        private bool isClearedFilterActive;
        private bool isRecurringFilterActive;
        private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);
        private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);

        /// <summary>
        /// Indicates wether the filter for only cleared Payments is active or not.
        /// </summary>
        public bool IsClearedFilterActive
        {
            get => isClearedFilterActive;
            set
            {
                if(isClearedFilterActive == value)
                    return;
                isClearedFilterActive = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Indicates wether the filter to only display recurring Payments is active or not.
        /// </summary>
        public bool IsRecurringFilterActive
        {
            get => isRecurringFilterActive;
            set
            {
                if(isRecurringFilterActive == value)
                    return;
                isRecurringFilterActive = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Start of the time range to load payments.
        /// </summary>
        public DateTime TimeRangeStart
        {
            get => timeRangeStart;
            set
            {
                if(timeRangeStart == value)
                    return;
                timeRangeStart = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// End of the time range to load payments.
        /// </summary>
        public DateTime TimeRangeEnd
        {
            get => timeRangeEnd;
            set
            {
                if(timeRangeEnd == value)
                    return;
                timeRangeEnd = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand FilterSelectedCommand => new RelayCommand(() => MessengerInstance.Send(new PaymentListFilterChangedMessage
        {
            IsClearedFilterActive = IsClearedFilterActive,
            IsRecurringFilterActive = IsRecurringFilterActive,
            TimeRangeStart = TimeRangeStart,
            TimeRangeEnd = TimeRangeEnd
        }));
    }
}
