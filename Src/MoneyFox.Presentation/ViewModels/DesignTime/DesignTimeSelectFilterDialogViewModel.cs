using System;
using System.Globalization;
using MoneyFox.Application.Resources;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSelectFilterDialogViewModel : ISelectFilterDialogViewModel
    {
        public DesignTimeSelectFilterDialogViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }
        
        public LocalizedResources Resources { get; }

        public bool IsClearedFilterActive { get; set; }
        public bool IsRecurringFilterActive { get; set; }
        public DateTime TimeRangeStart { get; set; }
        public DateTime TimeRangeEnd { get; set; }
    }
}
