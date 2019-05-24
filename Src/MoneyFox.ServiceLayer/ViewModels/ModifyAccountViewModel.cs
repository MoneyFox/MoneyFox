using System;
using System.Globalization;
using System.Threading.Tasks;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public abstract class ModifyAccountViewModel : BaseNavigationViewModel<ModifyAccountParameter>
    {
        private readonly IBackupService backupService;
        private readonly IMvxNavigationService navigationService;
        private readonly ISettingsFacade settingsFacade;

        protected int AccountId { get; private set; }

        private AccountViewModel selectedAccount;

        protected ModifyAccountViewModel(ISettingsFacade settingsFacade,
            IBackupService backupService,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
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
        
        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        public string AmountString
        {
            get => HelperFunctions.FormatLargeNumbers(SelectedAccount.CurrentBalance);
            set
            {
                // we remove all separator chars to ensure that it works in all regions
                string amountString = HelperFunctions.RemoveGroupingSeparators(value);
                if (double.TryParse(amountString, NumberStyles.Any, CultureInfo.CurrentCulture, out double convertedValue))
                {
                    SelectedAccount.CurrentBalance = convertedValue;
                }
            }
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