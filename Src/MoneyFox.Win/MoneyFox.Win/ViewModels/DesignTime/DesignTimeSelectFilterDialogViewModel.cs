namespace MoneyFox.Win.ViewModels.DesignTime;

using System;

public class DesignTimeSelectFilterDialogViewModel : ISelectFilterDialogViewModel
{
    public bool IsClearedFilterActive { get; set; }

    public bool IsRecurringFilterActive { get; set; }

    public DateTime TimeRangeStart { get; set; }

    public DateTime TimeRangeEnd { get; set; }
}