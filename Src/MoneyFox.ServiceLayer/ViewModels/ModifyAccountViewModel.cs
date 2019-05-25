using System;
using System.Globalization;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.Utilities;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public abstract class ModifyAccountViewModel : RouteableViewModelBase
    {
        private readonly IBackupService backupService;
        private readonly ISettingsFacade settingsFacade;
        private readonly IDialogService dialogService;

        private AccountViewModel selectedAccount;

        protected ModifyAccountViewModel(ISettingsFacade settingsFacade, IBackupService backupService, IDialogService dialogService)
        {
            this.settingsFacade = settingsFacade ?? Locator.Current.GetService<ISettingsFacade>();
            this.backupService = backupService ?? Locator.Current.GetService<IBackupService>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();

            this.WhenActivated(disposable =>
            {
                SaveCommand = ReactiveCommand.CreateFromTask(SaveAccountBase).DisposeWith(disposable);
                CancelCommand = ReactiveCommand.Create(Cancel).DisposeWith(disposable);
            });
        }

        public virtual string Title => Strings.AddAccountTitle;

        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set => this.RaiseAndSetIfChanged(ref selectedAccount, value);
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
            if (string.IsNullOrEmpty(SelectedAccount.Name)) {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }
            await SaveAccount();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            if (settingsFacade.IsBackupAutouploadEnabled)
            {
#pragma warning disable 4014
                backupService.EnqueueBackupTask();
#pragma warning restore 4014
            }
        }

        private void Cancel()
        {
            HostScreen.Router.NavigateBack.Execute();
        }
    }
}
