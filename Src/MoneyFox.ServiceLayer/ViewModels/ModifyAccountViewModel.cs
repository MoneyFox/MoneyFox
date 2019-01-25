using System;
using System.Globalization;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Parameters;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public interface IModifyAccountViewModel : IBaseViewModel
    {
        /// <summary>
        ///     indicates if the AccountViewModel already exists and shall
        ///     be updated or new created
        /// </summary>
        bool IsEdit { get; }
        
        /// <summary>
        ///     Returns the Title based on if the view is in edit mode or not.
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        string AmountString { get; }

        /// <summary>
        ///     The currently selected AccountViewModel
        /// </summary>
        AccountViewModel SelectedAccount { get; }

        /// <summary>
        ///     Saves all changes to the database
        ///     or creates a new AccountViewModel depending on
        ///     the <see cref="IsEdit" /> property
        /// </summary>
        /// 
        MvxAsyncCommand SaveCommand { get; }
        /// <summary>
        ///     Deletes the selected AccountViewModel from the database
        /// </summary>
        /// 
        MvxAsyncCommand DeleteCommand { get; }

        /// <summary>
        ///     Cancels the operation and will revert the changes
        /// </summary>
        MvxAsyncCommand CancelCommand { get; }
    }
   
    public abstract class ModifyAccountViewModel : BaseNavigationViewModel<ModifyAccountParameter>
    {
        private readonly IMvxNavigationService navigationService;

        private AccountViewModel selectedAccount;

        protected ModifyAccountViewModel(IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.navigationService = navigationService;
        }

        protected abstract Task SaveAccount();

        protected abstract Task DeleteAccount();

        public virtual string Title => Strings.AddAccountTitle;

        protected double Amount;

        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(SaveAccount);

        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeleteAccount);

        public MvxAsyncCommand CancelCommand => new MvxAsyncCommand(Cancel);
        

        public string AmountString
        {
            get => Utilities.Utilities.FormatLargeNumbers(Amount);
            set
            {
                // we remove all separator chars to ensure that it works in all regions
                string amountstring = Utilities.Utilities.RemoveGroupingSeparators(value);

                double convertedValue;
                if (double.TryParse(amountstring, NumberStyles.Any, CultureInfo.CurrentCulture, out convertedValue))
                {
                    Amount = convertedValue;
                }
            }
        }

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
            }
        }

        protected int AccountId;

        /// <inheritdoc />
        public override void Prepare(ModifyAccountParameter parameter)
        {
            AccountId = parameter.AccountId;
        }

//        private async Task SaveAccount()
//        {
//            if (string.IsNullOrEmpty(SelectedAccount.Name))
//            {
//                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
//                return;
//            }

//            if (!IsEdit && await accountService.CheckIfNameAlreadyTaken(SelectedAccount.Name))
//            {
//                await dialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateAccountMessage);
//                return;
//            }

//            SelectedAccount.CurrentBalance = amount;

//            await accountService.SaveAccount(SelectedAccount.Account);
//            settingsManager.LastDatabaseUpdate = DateTime.Now;
//#pragma warning disable 4014
//            backupManager.EnqueueBackupTask();
//#pragma warning restore 4014
//            await navigationService.Close(this);
//        }

//        private async Task DeleteAccount()
//        {
//            try
//            {
//                await accountService.DeleteAccount(SelectedAccount.Account);
//                settingsManager.LastDatabaseUpdate = DateTime.Now;
//#pragma warning disable 4014
//                backupManager.EnqueueBackupTask();
//#pragma warning restore 4014
//                await navigationService.Close(this);
//            } 
//            catch(Exception ex)
//            {
//                Crashes.TrackError(ex);
//                await dialogService.ShowMessage(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
//            }
//        }

        private async Task Cancel()
        {
            await navigationService.Close(this);
        }
    }
}