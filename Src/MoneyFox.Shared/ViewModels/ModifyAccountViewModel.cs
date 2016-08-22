using System;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyAccountViewModel : BaseViewModel {
        private readonly IRepository<Account> accountRepository;
        private readonly IDialogService dialogService;

        public ModifyAccountViewModel(IRepository<Account> accountRepository, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.accountRepository = accountRepository;
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

        /// <summary>
        ///     Returns the Title based on if the view is in edit mode or not.
        /// </summary>
        public string Title => IsEdit
            ? string.Format(Strings.EditAccountTitle, SelectedAccount.Name)
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
        public Account SelectedAccount { get; set; }

        public void Init(bool isEdit)
        {
            IsEdit = isEdit;

            if (!IsEdit)
            {
                SelectedAccount = new Account();
            }
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
                ? accountRepository.GetList(x => x.Id == selectedAccountId).First()
                : new Account();
        }

        private async void SaveAccount()
        {
            if (string.IsNullOrEmpty(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (!IsEdit && accountRepository.GetList(a => string.Equals(a.Name, SelectedAccount.Name, StringComparison.CurrentCultureIgnoreCase)).Any())
            {
                await dialogService.ShowMessage(Strings.ErrorMessageSave, Strings.DuplicateAccountMessage);
                return;
            }

            if (accountRepository.Save(SelectedAccount))
            {
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;
                Close(this);
            }
        }

        private void DeleteAccount()
        {
            if (accountRepository.Delete(SelectedAccount))
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;
            Close(this);
        }

        private void Cancel()
        {
            //TODO: revert changes
            Close(this);
        }
    }
}