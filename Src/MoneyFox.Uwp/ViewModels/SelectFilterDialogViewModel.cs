using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MoneyFox.Application.Common.Messages;
using System;

#nullable enable
namespace MoneyFox.Uwp.ViewModels
{
    public class SelectFilterDialogViewModel : ObservableRecipient, ISelectFilterDialogViewModel
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
                {
                    return;
                }

                isClearedFilterActive = value;
                OnPropertyChanged();
                UpdateList();
            }
        }

        /// <summary>
        /// Indicates whether the filter to only display recurring Payments is active or not.
        /// </summary>
        public bool IsRecurringFilterActive
        {
            get => isRecurringFilterActive;
            set
            {
                if(isRecurringFilterActive == value)
                {
                    return;
                }

                isRecurringFilterActive = value;
                OnPropertyChanged();
                UpdateList();
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
                {
                    return;
                }

                timeRangeStart = value;
                OnPropertyChanged();
                UpdateList();
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
                {
                    return;
                }

                timeRangeEnd = value;
                OnPropertyChanged();
                UpdateList();
            }
        }

        private void UpdateList() =>
            Messenger.Send(new PaymentListFilterChangedMessage
            {
                IsClearedFilterActive = IsClearedFilterActive,
                IsRecurringFilterActive = IsRecurringFilterActive,
                TimeRangeStart = TimeRangeStart,
                TimeRangeEnd = TimeRangeEnd
            });
    }
}