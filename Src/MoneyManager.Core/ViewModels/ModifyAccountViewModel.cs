using System.Linq;
using MoneyManager.Core.Helpers;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyAccountViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;

        public ModifyAccountViewModel(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Init(bool isEdit)
        {
            IsEdit = isEdit;

            if (!IsEdit)
            {
                SelectedAccount = new Account();
            }
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
            set
            {
                double amount;
                if (double.TryParse(value, out amount))
                {
                    SelectedAccount.CurrentBalance = amount;
                }
            }
        }

        /// <summary>
        ///     The currently selected account
        /// </summary>
        public Account SelectedAccount
        {
            get { return accountRepository.Selected; }
            set { accountRepository.Selected = value; }
        }

        /// <summary>
        ///     Initializes the ViewModel
        /// </summary>
        /// <param name="isEdit">Indicates if the view is in edit or create mode.</param>
        /// <param name="selectedAccountId">if in edit mode, this is the selected account.</param>
        public void Init(bool isEdit, int selectedAccountId)
        {
            IsEdit = isEdit;
            SelectedAccount = selectedAccountId != 0
                ? accountRepository.Data.First(x => x.Id == selectedAccountId)
                : new Account();
        }

        private void SaveAccount()
        {
            accountRepository.Save(accountRepository.Selected);
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