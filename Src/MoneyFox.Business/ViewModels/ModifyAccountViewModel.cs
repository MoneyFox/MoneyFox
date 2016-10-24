using System;
using System.Linq;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class ModifyAccountViewModel : BaseViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDialogService dialogService;
        private readonly ISettingsManager settingsManager;
        private bool isEdit;
        private AccountViewModel selectedAccount;

        public ModifyAccountViewModel(IAccountRepository accountRepository, IDialogService dialogService,
            ISettingsManager settingsManager)
        {
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        ///     Saves all changes to the database
        ///     or creates a new AccountViewModel depending on
        ///     the <see cref="IsEdit" /> property
        /// </summary>
        public MvxCommand SaveCommand => new MvxCommand(SaveAccount);

        /// <summary>
        ///     Deletes the selected AccountViewModel from the database
        /// </summary>
        public MvxCommand DeleteCommand => new MvxCommand(DeleteAccount);

        /// <summary>
        ///     Cancels the operation and will revert the changes
        /// </summary>
        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     indicates if the AccountViewModel already exists and shall
        ///     be updated or new created
        /// </summary>
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The currently selected AccountViewModel
        /// </summary>
        public AccountViewModel SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
            }
        }

        public void Init(bool isEdit)
        {
            IsEdit = isEdit;

            if (!IsEdit)
            {
                SelectedAccount = new AccountViewModel();
            }
        }

        /// <summary>
        ///     Initializes the ViewModel
        /// </summary>
        /// <param name="isEdit">Indicates if the view is in edit or create mode.</param>
        /// <param name="selectedAccountId">if in edit mode, this is the selected AccountViewModel.</param>
        public void Init(bool isEdit, int selectedAccountId)
        {
            IsEdit = isEdit;
            SelectedAccount = selectedAccountId != 0
                ? accountRepository.GetList(x => x.Id == selectedAccountId).First()
                : new AccountViewModel();
        }

        private async void SaveAccount()
        {
            if (string.IsNullOrEmpty(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (!IsEdit &&
                accountRepository.GetList(
                    a => string.Equals(a.Name, SelectedAccount.Name, StringComparison.CurrentCultureIgnoreCase)).Any())
            {
                await dialogService.ShowMessage(Strings.ErrorMessageSave, Strings.DuplicateAccountMessage);
                return;
            }

            if (accountRepository.Save(SelectedAccount))
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;
                Close(this);
            }
        }

        private void DeleteAccount()
        {
            if (accountRepository.Delete(SelectedAccount))
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;
            }
            Close(this);
        }

        private void Cancel()
        {
            SelectedAccount = accountRepository.FindById(SelectedAccount.Id);
            Close(this);
        }
    }
}