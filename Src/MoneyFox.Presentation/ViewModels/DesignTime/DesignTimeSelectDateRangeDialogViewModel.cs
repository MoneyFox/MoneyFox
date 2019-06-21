using System;
using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeSelectDateRangeDialogViewModel : ISelectDateRangeDialogViewModel
    {
        public DesignTimeSelectDateRangeDialogViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        public LocalizedResources Resources { get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MvxCommand DoneCommand { get; set; }
    }
}
