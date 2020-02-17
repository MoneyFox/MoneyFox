using System;
using GalaSoft.MvvmLight.Command;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSelectDateRangeDialogViewModel : ISelectDateRangeDialogViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RelayCommand DoneCommand { get; set; }
    }
}
