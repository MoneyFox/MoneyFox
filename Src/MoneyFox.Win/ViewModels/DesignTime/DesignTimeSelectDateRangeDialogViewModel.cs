using CommunityToolkit.Mvvm.Input;
using System;

namespace MoneyFox.Win.ViewModels.DesignTime
{
    public class DesignTimeSelectDateRangeDialogViewModel : ISelectDateRangeDialogViewModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public RelayCommand DoneCommand { get; set; } = null!;
    }
}