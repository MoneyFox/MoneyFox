using System;
using System.Globalization;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.Utilities;
using HelperFunctions = MoneyFox.Presentation.Utilities.HelperFunctions;

namespace MoneyFox.Presentation.ViewModels
{
    public abstract class ModifyAccountViewModel : BaseViewModel
    {
        private readonly IBackupService backupService;
        private readonly ISettingsFacade settingsFacade;

        public int AccountId { get; set; }

        private string title;
        private AccountViewModel selectedAccount = new AccountViewModel();

        protected ModifyAccountViewModel(ISettingsFacade settingsFacade,
            IBackupService backupService,
            INavigationService navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            NavigationService = navigationService;
        }

        protected abstract Task SaveAccount();

        protected abstract Task Initialize();


        protected INavigationService NavigationService { get; private set; }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public AsyncCommand SaveCommand => new AsyncCommand(SaveAccountBase);

        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        public string Title
        {
            get => title;
            set
            {
                if (title == value) return;
                title = value;
                RaisePropertyChanged();
            }
        }

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(AmountString));
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

        private async Task SaveAccountBase()
        {
            await SaveAccount();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            if (settingsFacade.IsBackupAutouploadEnabled)
            {
                backupService.EnqueueBackupTask().FireAndForgetSafeAsync();
            }
        }
        
        private void Cancel()
        {
            NavigationService.GoBack();
        }
    }
}