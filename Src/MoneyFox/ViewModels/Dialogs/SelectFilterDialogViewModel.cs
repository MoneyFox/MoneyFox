namespace MoneyFox.ViewModels.Dialogs
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using System;
    using System.Collections.Generic;

    public interface ISelectFilterDialogViewModel
    {
        bool IsClearedFilterActive { get; set; }

        bool IsRecurringFilterActive { get; set; }

        int PaymentTypeFilter { get; set; }

        DateTime TimeRangeStart { get; set; }

        DateTime TimeRangeEnd { get; set; }
    }

    public class SelectFilterDialogViewModel : ObservableRecipient, ISelectFilterDialogViewModel
    {
        private bool isClearedFilterActive;
        private bool isRecurringFilterActive;
        private int paymentTypeFilter = -1;
        private DateTime timeRangeEnd = DateTime.Now.AddMonths(6);
        private DateTime timeRangeStart = DateTime.Now.AddMonths(-2);

        public List<int> PaymentTypeFilterList => new List<int>() { -1, 0, 1, 2 };

        public RelayCommand FilterSelectedCommand => new RelayCommand(
            () =>
                Messenger.Send(
                    new PaymentListFilterChangedMessage
                    {
                        IsClearedFilterActive = IsClearedFilterActive,
                        IsRecurringFilterActive = IsRecurringFilterActive,
                        TimeRangeStart = TimeRangeStart,
                        TimeRangeEnd = TimeRangeEnd,
                        PaymentTypeFilter = PaymentTypeFilter
                    }));

        /// <summary>
        ///     Indicates whether the filter for only cleared Payments is active or not.
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
            }
        }

        /// <summary>
        ///     Indicates whether the filter to only display recurring Payments is active or not.
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
            }
        }

        /// <summary>
        ///     Indicates whether the filter to only display a specific payment type is active or not.
        /// </summary>
        public int PaymentTypeFilter
        {
            get => paymentTypeFilter;
            set
            {
                if(paymentTypeFilter == value)
                {
                    return;
                }

                paymentTypeFilter = value;
                OnPropertyChanged();
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
                if(timeRangeStart == value)
                {
                    return;
                }

                timeRangeStart = value;
                OnPropertyChanged();
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
                if(timeRangeEnd == value)
                {
                    return;
                }

                timeRangeEnd = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Initalize the viewmodel with a previous sent message.
        /// </summary>
        public void Initialize(PaymentListFilterChangedMessage message)
        {
            IsClearedFilterActive = message.IsClearedFilterActive;
            IsRecurringFilterActive = message.IsRecurringFilterActive;
            TimeRangeStart = message.TimeRangeStart;
            TimeRangeEnd = message.TimeRangeEnd;
            PaymentTypeFilter = message.PaymentTypeFilter;
        }
    }
}