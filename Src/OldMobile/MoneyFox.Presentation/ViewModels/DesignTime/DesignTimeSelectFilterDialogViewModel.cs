using System;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSelectFilterDialogViewModel : ISelectFilterDialogViewModel
    {
        public bool IsClearedFilterActive { get; set; }

        public bool IsRecurringFilterActive { get; set; }

        public DateTime TimeRangeStart { get; set; }

        public DateTime TimeRangeEnd { get; set; }
    }
}
