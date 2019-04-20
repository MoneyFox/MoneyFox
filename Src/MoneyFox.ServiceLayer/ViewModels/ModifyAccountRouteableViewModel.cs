using System;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using ReactiveUI;

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

    public abstract class ModifyAccountRouteableViewModel : RouteableViewModelBase
    {
        private readonly IBackupService backupService;
        private readonly ISettingsFacade settingsFacade;

        private AccountViewModel selectedAccount;

        protected ModifyAccountRouteableViewModel(ISettingsFacade settingsFacade,
            IBackupService backupService)
        {
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
        }

        public virtual string Title => Strings.AddAccountTitle;

        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(SaveAccountBase);

        public MvxAsyncCommand CancelCommand => new MvxAsyncCommand(Cancel);

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set => this.RaiseAndSetIfChanged(ref selectedAccount, value);
        }

        protected abstract Task SaveAccount();

        private async Task SaveAccountBase()
        {
            await SaveAccount();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
        }

        ///// <inheritdoc />
        //public override void Prepare(ModifyAccountParameter parameter)
        //{
        //    AccountId = parameter.AccountId;
        //}

        private async Task Cancel()
        {
            HostScreen.Router.NavigateBack.Execute();
        }
    }
}