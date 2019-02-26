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
        private readonly IPaymentService paymentService;
        private readonly IBackupService backupService;
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public EditPaymentViewModel(IPaymentService paymentService,
            ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade, 
            IMvxMessenger messenger,
            IBackupService backupService,
            IMvxLogProvider logProvider, 
            IMvxNavigationService navigationService) 
            : base(crudServices, dialogService, settingsFacade, messenger, backupService, logProvider, navigationService)
        {
            this.paymentService = paymentService;
            this.crudServices = crudServices;
            this.dialogService = dialogService;
            this.backupService = backupService;
            this.paymentService = paymentService;
        }

        public override void Prepare(ModifyPaymentParameter parameter)
        {
            SelectedPayment = crudServices.ReadSingleAsync<PaymentViewModel>(parameter.PaymentId).Result;

            // We have to set this here since otherwise the end date is null. This causes a crash on android.
            // Also it's user unfriendly if you the default end date is the 1.1.0001.
            if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment.IsEndless)
            {
                SelectedPayment.RecurringPayment.EndDate = DateTime.Today;
            }

            Title = PaymentTypeHelper.GetViewTitleForType(parameter.PaymentType, true);
            base.Prepare(parameter);
        }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeletePayment);
        
        protected override async Task SavePayment()
        {
            var result = await paymentService.UpdatePayment(SelectedPayment)
                                .ConfigureAwait(true);

            if (!result.Success)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors())
                    .ConfigureAwait(true);
                return;
            }

            await NavigationService.Close(this).ConfigureAwait(true);
        }


        private async Task DeletePayment()
        {
            await paymentService.DeletePayment(SelectedPayment).ConfigureAwait(true);
#pragma warning disable 4014
            backupService.EnqueueBackupTask().ConfigureAwait(true);
#pragma warning restore 4014
            await CancelCommand.ExecuteAsync().ConfigureAwait(true);
        }
    }
}