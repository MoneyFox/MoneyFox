using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public abstract class ModifyAccountViewModel : RouteableViewModelBase
    {
        private readonly IBackupService backupService;
        private readonly ISettingsFacade settingsFacade;

        private AccountViewModel selectedAccount;

        protected ModifyAccountViewModel(ISettingsFacade settingsFacade, IBackupService backupService)
        {
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;

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

        protected abstract Task SaveAccount();

        private async Task SaveAccountBase()
        {
            await SaveAccount();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
        }

        private void Cancel()
        {
            HostScreen.Router.NavigateBack.Execute();
        }
    }
}