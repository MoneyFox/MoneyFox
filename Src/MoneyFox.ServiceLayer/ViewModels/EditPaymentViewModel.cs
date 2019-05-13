using System;
using System.Reactive;
using System.Reactive.Disposables;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using ReactiveUI;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Utilities;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class EditPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IPaymentService paymentService;
        private readonly IBackupService backupService;
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public EditPaymentViewModel(ModifyPaymentParameter parameter,
                                    IScreen screen = null,
                                    IPaymentService paymentService = null,
                                    ICrudServicesAsync crudServices = null,
                                    IDialogService dialogService = null,
                                    ISettingsFacade settingsFacade = null,
                                    IBackupService backupService = null)
            : base(screen, crudServices, dialogService, settingsFacade, backupService)
        {
            this.paymentService = paymentService ?? Locator.Current.GetService<IPaymentService>();
            this.crudServices = crudServices ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            this.backupService = backupService ?? Locator.Current.GetService<IBackupService>();

            this.WhenActivated(async disposable =>
            {
                SelectedPayment = await this.crudServices.ReadSingleAsync<PaymentViewModel>(parameter.PaymentId);
                //We have to set this here since otherwise the end date is null.This causes a crash on android.
                // Also it's user unfriendly if you the default end date is the 1.1.0001.
                if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment.IsEndless)
                {
                    SelectedPayment.RecurringPayment.EndDate = DateTime.Today;
                }

                Title = PaymentTypeHelper.GetViewTitleForType(parameter.PaymentType, true);

                DeleteCommand = ReactiveCommand.CreateFromTask(DeletePayment)
                                               .DisposeWith(disposable);
            });
        }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; set; }

        public override string UrlPathSegment => "EditPayment";

        protected override async Task SavePayment()
        {
            OperationResult result = await paymentService.UpdatePayment(SelectedPayment);

            if (!result.Success)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
                return;
            }

            HostScreen.Router.NavigateBack.Execute();
        }


        private async Task DeletePayment()
        {
            await paymentService.DeletePayment(SelectedPayment);
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            CancelCommand.Execute();
        }
    }
}