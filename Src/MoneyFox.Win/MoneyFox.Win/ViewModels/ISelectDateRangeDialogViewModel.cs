namespace MoneyFox.Win.ViewModels;

using CommunityToolkit.Mvvm.Input;
using System;

public interface ISelectDateRangeDialogViewModel
{
    DateTime StartDate { get; set; }

    DateTime EndDate { get; set; }

    RelayCommand DoneCommand { get; set; }
}