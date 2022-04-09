namespace MoneyFox.Win.ViewModels;

using System;
using CommunityToolkit.Mvvm.Input;

public interface ISelectDateRangeDialogViewModel
{
    DateTime StartDate { get; set; }

    DateTime EndDate { get; set; }

    RelayCommand DoneCommand { get; set; }
}
