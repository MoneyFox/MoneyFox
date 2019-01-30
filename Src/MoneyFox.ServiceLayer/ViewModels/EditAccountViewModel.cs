using System;
using System.Globalization;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class EditAccountViewModel : ModifyAccountViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IBackupService backupService;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;

        public EditAccountViewModel(ICrudServicesAsync crudServices,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            IDialogService dialogService,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) : base(settingsFacade, backupService, logProvider,
            navigationService)
        {
            this.crudServices = crudServices;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.dialogService = dialogService;
        }

        public override string Title =>
            string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);

        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeleteAccount);

        public override async void Prepare(ModifyAccountParameter parameter)
        {
            base.Prepare(parameter);
            SelectedAccount = await crudServices.ReadSingleAsync<AccountViewModel>(AccountId)
                                                .ConfigureAwait(true);
        }

        protected override async Task SaveAccount()
        {
            await crudServices.UpdateAndSaveAsync(SelectedAccount)
                              .ConfigureAwait(true);
            if (!crudServices.IsValid)
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors())
                                   .ConfigureAwait(true);

            await NavigationService.Close(this).ConfigureAwait(true);
        }

        protected async Task DeleteAccount()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedAccount.Id)
                              .ConfigureAwait(true);

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask().ConfigureAwait(true);
#pragma warning restore 4014
        }
    }
}