using System;
using GalaSoft.MvvmLight.Command;

namespace MoneyFox.Presentation.ViewModels
{
    public interface ISelectDateRangeDialogViewModel
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }

        RelayCommand DoneCommand { get; set; }
    }
}