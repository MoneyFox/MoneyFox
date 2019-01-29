using System;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class EditPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupService backupService;

        public EditPaymentViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade, 
            IMvxMessenger messenger,
            IBackupService backupService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) 
            : base(crudServices, dialogService, settingsFacade, messenger, backupService, logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
        }

        public override void Prepare(ModifyPaymentParameter parameter)
        {
            SelectedPayment = crudServices.ReadSingleAsync<PaymentViewModel>(parameter.PaymentId).Result;
            Title = PaymentTypeHelper.GetViewTitleForType(parameter.PaymentType, true);
            base.Prepare(parameter);
        }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeletePayment);
        
        protected override async Task SavePayment()
        {
            await crudServices.UpdateAndSaveAsync(SelectedPayment, "ctor(7)")
                              .ConfigureAwait(true);
            if (!crudServices.IsValid)
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors())
                                   .ConfigureAwait(true);

            await NavigationService.Close(this).ConfigureAwait(true);
        }


        private async Task DeletePayment()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedPayment.Id)
                              .ConfigureAwait(true);

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            await backupService.EnqueueBackupTask().ConfigureAwait(true);
        }
    }
}