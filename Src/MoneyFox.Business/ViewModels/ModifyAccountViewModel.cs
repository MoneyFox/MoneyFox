using System;
using System.Globalization;
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
        private readonly IBackupManager backupManager;

        private bool isEdit;
        private double amount;
        private AccountViewModel selectedAccount;

        public ModifyAccountViewModel(IAccountRepository accountRepository, IDialogService dialogService,
            ISettingsManager settingsManager,
            IBackupManager backupManager)
        {
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.backupManager = backupManager;
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
            get { return Utilities.FormatLargeNumbers(amount); }
            set
            {
                // we replace the separator char to ensure that it works in all regions
                var amountstring = value.Replace(',', '.');

                double convertedValue;
                if (double.TryParse(amountstring, NumberStyles.Any, CultureInfo.InvariantCulture, out convertedValue))
                {
                    amount = convertedValue;
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

        /// <summary>
        ///     Initializes the ViewModel
        /// </summary>
        /// <param name="accountId">Pass the ID of the account to edit. If this is 0 the VM changes to Creation mode</param>
        public void Init(int accountId = 0)
        {
            if (accountId == 0)
            {
                IsEdit = false;
                amount = 0;
                SelectedAccount = new AccountViewModel();
            }
            else
            {
                IsEdit = true;
                SelectedAccount = accountRepository.FindById(accountId);
                amount = SelectedAccount.CurrentBalance;
            }
        }

        private async void SaveAccount()
        {
            if (string.IsNullOrEmpty(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            SelectedAccount.CurrentBalance = amount;

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
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014
                Close(this);
            }
        }

        private void DeleteAccount()
        {
            if (accountRepository.Delete(SelectedAccount))
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014

            }
            Close(this);
        }

        private void Cancel()
        {
            Close(this);
        }
    }
}