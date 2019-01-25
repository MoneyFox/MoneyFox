using System;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.QueryObject;
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
        MvxAsyncCommand SaveCommand { get; }

        /// <summary>
        ///     Deletes the selected AccountViewModel from the database
        /// </summary>
        MvxAsyncCommand DeleteCommand { get; }

        /// <summary>
        ///     Cancels the operation and will revert the changes
        /// </summary>
        MvxAsyncCommand CancelCommand { get; }
    }

    public abstract class ModifyAccountViewModel : BaseNavigationViewModel<ModifyAccountParameter>
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupManager backupManager;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;

        protected int AccountId;

        private AccountViewModel selectedAccount;

        protected ModifyAccountViewModel(ICrudServicesAsync crudServices,
            ISettingsFacade settingsFacade,
            IBackupManager backupManager,
            IDialogService dialogService,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.settingsFacade = settingsFacade;
            this.backupManager = backupManager;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
        }

        public virtual string Title => Strings.AddAccountTitle;

        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(SaveAccountBase);

        public MvxAsyncCommand CancelCommand => new MvxAsyncCommand(Cancel);

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
            }
        }

        protected abstract Task SaveAccount();

        private async Task SaveAccountBase()
        {
            if (await crudServices.ReadManyNoTracked<AccountViewModel>().AnyWithName(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await SaveAccount();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            await backupManager.EnqueueBackupTask();
        }

        /// <inheritdoc />
        public override void Prepare(ModifyAccountParameter parameter)
        {
            AccountId = parameter.AccountId;
        }

        private async Task Cancel()
        {
            await navigationService.Close(this);
        }
    }
}