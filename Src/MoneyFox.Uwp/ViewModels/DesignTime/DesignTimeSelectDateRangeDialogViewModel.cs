using GalaSoft.MvvmLight.Command;
using System;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeSelectDateRangeDialogViewModel : ISelectDateRangeDialogViewModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public RelayCommand DoneCommand { get; set; } = null!;
    }
}
