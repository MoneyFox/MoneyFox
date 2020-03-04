using System;

namespace MoneyFox.Uwp.ViewModels
{
    public interface ISelectFilterDialogViewModel
    {
        bool IsClearedFilterActive { get; set; }

        bool IsRecurringFilterActive { get; set; }

        DateTime TimeRangeStart { get; set; }

        DateTime TimeRangeEnd { get; set; }
    }
}
