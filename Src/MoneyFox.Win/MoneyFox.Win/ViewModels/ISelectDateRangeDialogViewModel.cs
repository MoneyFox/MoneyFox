using CommunityToolkit.Mvvm.Input;
using System;

#nullable enable
namespace MoneyFox.Win.ViewModels
{
    public interface ISelectDateRangeDialogViewModel
    {
        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }

        RelayCommand DoneCommand { get; set; }
    }
}