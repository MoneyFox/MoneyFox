namespace MoneyFox.Win.ViewModels.DesignTime;

using System;
using CommunityToolkit.Mvvm.Input;

public class DesignTimeSelectDateRangeDialogViewModel : ISelectDateRangeDialogViewModel
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public RelayCommand DoneCommand { get; set; } = null!;
}
