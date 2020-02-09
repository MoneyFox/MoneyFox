using System;
using System.Globalization;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Services;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Utilities;
using NLog;

namespace MoneyFox.Presentation.ViewModels
{
    public abstract class ModifyAccountViewModel : ViewModelBase
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IBackupService backupService;
        private readonly ISettingsFacade settingsFacade;

        public int AccountId { get; set; }

        private string title;
        private AccountViewModel selectedAccount = new AccountViewModel();

        protected ModifyAccountViewModel(ISettingsFacade settingsFacade,
                                         IBackupService backupService,
                                         IDialogService dialogService,
                                         INavigationService navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;

            DialogService = dialogService;
            NavigationService = navigationService;
        }

        protected abstract Task SaveAccount();

        protected abstract Task Initialize();

        protected IDialogService DialogService { get; }
        protected INavigationService NavigationService { get; }

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

        private string amountString;

        public string AmountString
        {
            get => amountString;
            set
            {
                if (amountString == value) return;
                amountString = value;
                RaisePropertyChanged();
            }
        }

        private async Task SaveAccountBase()
        {
            if (string.IsNullOrWhiteSpace(SelectedAccount.Name))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (decimal.TryParse(AmountString, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal convertedValue))
                SelectedAccount.CurrentBalance = convertedValue;
            else
            {
                logManager.Warn($"Amount string {AmountString} could not be parsed to double.");
                await DialogService.ShowMessageAsync(Strings.InvalidNumberTitle, Strings.InvalidNumberCurrentBalanceMessage);
                return;
            }

            await SaveAccount();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            if (settingsFacade.IsBackupAutouploadEnabled) backupService.UploadBackupAsync().FireAndForgetSafeAsync();
        }

        private void Cancel()
        {
            NavigationService.GoBack();
        }
    }
}
