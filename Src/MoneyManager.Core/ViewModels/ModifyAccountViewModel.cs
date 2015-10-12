using System;
using System.Globalization;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyAccountViewModel : BaseViewModel
    {
        private readonly IRepository<Account> accountRepository;
        private readonly BalanceViewModel balanceViewModel;

        public ModifyAccountViewModel(IRepository<Account> accountRepository, BalanceViewModel balanceViewModel)
        {
            this.accountRepository = accountRepository;
            this.balanceViewModel = balanceViewModel;
        }

        /// <summary>
        ///     Saves all changes to the database
        ///     or creates a new account depending on
        ///     the <see cref="IsEdit" /> property
        /// </summary>
        public MvxCommand SaveCommand => new MvxCommand(SaveAccount);

        /// <summary>
        ///     Deletes the selected account from the database
        /// </summary>
        public MvxCommand DeleteCommand => new MvxCommand(DeleteAccount);

        /// <summary>
        ///     Cancels the operation and will revert the changes
        /// </summary>
        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     indicates if the account already exists and shall
        ///     be updated or new created
        /// </summary>
        public bool IsEdit { get; set; }

        public string Title => IsEdit
            ? $"{Strings.EditTitle} {SelectedAccount.Name}"
            : Strings.AddAccountTitle;

        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        public string AmountString
        {
            get { return Utilities.FormatLargeNumbers(SelectedAccount.CurrentBalance); }
            set { SelectedAccount.CurrentBalance = Convert.ToDouble(value, CultureInfo.CurrentCulture); }
        }

        /// <summary>
        ///     The currently selected account
        /// </summary>
        public Account SelectedAccount
        {
            get { return accountRepository.Selected; }
            set { accountRepository.Selected = value; }
        }

        private void SaveAccount()
        {
            accountRepository.Save(accountRepository.Selected);
            balanceViewModel.UpdateBalance();

            Close(this);
        }

        private void DeleteAccount()
        {
            accountRepository.Delete(accountRepository.Selected);
            Close(this);
        }

        private void Cancel()
        {
            //TODO: revert changes
            Close(this);
        }
    }
}