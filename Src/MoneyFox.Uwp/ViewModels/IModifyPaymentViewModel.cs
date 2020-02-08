using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Domain;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels
{
    public interface IModifyPaymentViewModel
    {
        /// <summary>
        ///     Indicates if the PaymentViewModel is a transfer.
        /// </summary>
        bool IsTransfer { get; }

        /// <summary>
        ///     The selected recurrence
        /// </summary>
        PaymentRecurrence Recurrence { get; }

        /// <summary>
        ///     List with the different recurrence types.
        ///     This has to have the same order as the enum
        /// </summary>
        List<PaymentRecurrence> RecurrenceList { get; }

        /// <summary>
        ///     The selected PaymentViewModel
        /// </summary>
        PaymentViewModel SelectedPayment { get; }

        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        string AmountString { get; }

        /// <summary>
        ///     Gives access to all accounts for Charged Drop down list
        /// </summary>
        ObservableCollection<AccountViewModel> ChargedAccounts { get; }

        /// <summary>
        ///     Gives access to all accounts for Target Drop down list
        /// </summary>
        ObservableCollection<AccountViewModel> TargetAccounts { get; }

        /// <summary>
        ///     Returns the Title for the page
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     Returns the Header for the AccountViewModel field
        /// </summary>
        string AccountHeader { get; }

        /// <summary>
        ///     Updates the targetAccountViewModel and chargedAccountViewModel Comboboxes' dropdown lists.
        /// </summary>
        RelayCommand SelectedItemChangedCommand { get; }

        /// <summary>
        ///     Saves the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        AsyncCommand SaveCommand { get; }

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        RelayCommand GoToSelectCategoryDialogCommand { get; }

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        RelayCommand CancelCommand { get; }

        /// <summary>
        ///     Resets the CategoryViewModel of the currently selected PaymentViewModel
        /// </summary>
        RelayCommand ResetCategoryCommand { get; }
    }
}