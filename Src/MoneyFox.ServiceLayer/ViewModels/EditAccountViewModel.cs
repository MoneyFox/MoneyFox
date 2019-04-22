using System;
using System.Globalization;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Commands;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IBackupService backupService;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;

        public EditAccountViewModel(IScreen hostScreen,
            int accountId,
            ICrudServicesAsync crudServices = null,
            ISettingsFacade settingsFacade = null,
            IBackupService backupService = null,
            IDialogService dialogService = null) : base(settingsFacade, backupService)
        {
            HostScreen = hostScreen;

            this.crudServices = crudServices ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.settingsFacade = settingsFacade ?? Locator.Current.GetService<ISettingsFacade>();
            this.backupService = backupService ?? Locator.Current.GetService<IBackupService>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();

            this.WhenActivated(async (CompositeDisposable disposable) =>
            {
                SelectedAccount = await this.crudServices.ReadSingleAsync<AccountViewModel>(accountId);
            });
        }

        public override string Title =>
            string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);

        public override string UrlPathSegment => "EditAccount";
        public override IScreen HostScreen { get; }

        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeleteAccount);

        protected override async Task SaveAccount()
        {
            await crudServices.UpdateAndSaveAsync(SelectedAccount);

            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            HostScreen.Router.NavigateBack.Execute();
        }

        protected async Task DeleteAccount()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedAccount.Id);

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            HostScreen.Router.NavigateBack.Execute();
        }
    }
}