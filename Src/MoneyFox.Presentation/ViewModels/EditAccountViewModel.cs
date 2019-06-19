using System;
using System.Globalization;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Parameters;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
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
            INavigationService navigationService) : base(settingsFacade, backupService, navigationService)
        {
            this.crudServices = crudServices;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.dialogService = dialogService;
        }

        public override string Title =>
            string.Format(CultureInfo.InvariantCulture, Strings.EditAccountTitle, SelectedAccount.Name);

        public RelayCommand DeleteCommand => new RelayCommand(DeleteAccount);
        public RelayCommand InitializeCommand => new RelayCommand(Initialize);

        public async void Initialize()
        {
            SelectedAccount = await crudServices.ReadSingleAsync<AccountViewModel>(AccountId);
        }

        protected override async Task SaveAccount()
        {
            await crudServices.UpdateAndSaveAsync(SelectedAccount);

            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            CancelCommand.Execute(null);
        }

        protected async void DeleteAccount()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedAccount.Id);

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            CancelCommand.Execute(null);
        }
    }
}