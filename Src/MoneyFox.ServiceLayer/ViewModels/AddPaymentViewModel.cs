using System;
using GenericServices;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using ReactiveUI;
using Splat;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IPaymentService paymentService;
        private readonly IDialogService dialogService;

        public AddPaymentViewModel(PaymentType paymentType,
                                   IScreen screen,
                                   IPaymentService paymentService = null,
                                   ICrudServicesAsync crudServices = null,
                                   IDialogService dialogService = null,
                                   ISettingsFacade settingsFacade = null,
                                   IBackupService backupService = null)
            : base(screen, crudServices, dialogService, settingsFacade, backupService)
        {
            this.paymentService = paymentService ?? Locator.Current.GetService<IPaymentService>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();

            this.WhenActivated(async disposable =>
            {
                SelectedPayment = new PaymentViewModel
                {
                    Type = paymentType
                };
                
                // //We have to set this here since otherwise the end date is null.This causes a crash on android.
                // Also it's user unfriendly if you the default end date is the 1.1.0001.
                if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment.IsEndless)
                    SelectedPayment.RecurringPayment.EndDate = DateTime.Today;

                Title = PaymentTypeHelper.GetViewTitleForType(paymentType, false);

                SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();

                if (SelectedPayment.IsTransfer)
                {
                    SelectedItemChangedCommand.Execute();
                    SelectedPayment.TargetAccount = TargetAccounts.FirstOrDefault();
                }

                DeleteCommand = ReactiveCommand.CreateFromTask(DeletePayment)
                                               .DisposeWith(disposable);
            });
        }

            SelectedPayment = new PaymentViewModel
            {
                Type = paymentType
            };

            this.WhenActivated((CompositeDisposable disposable) =>
            {
                // //We have to set this here since otherwise the end date is null.This causes a crash on android.
                // Also it's user unfriendly if you the default end date is the 1.1.0001.
                if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment.IsEndless)
                {
                    SelectedPayment.RecurringPayment.EndDate = DateTime.Today;
                }

                Title = PaymentTypeHelper.GetViewTitleForType(paymentType, false);

                SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();

                if (SelectedPayment.IsTransfer)
                {
                    SelectedItemChangedCommand.Execute();
                    SelectedPayment.TargetAccount = TargetAccounts.FirstOrDefault();
                }
            });
        }

        public override string UrlPathSegment => "AddPayment";

        protected override async Task SavePayment()
        {
            try
            {
                OperationResult result = await paymentService.SavePayment(SelectedPayment);

                if (!result.Success)
                {
                    await dialogService.ShowMessage(Strings.GeneralErrorTitle, result.Message);
                    return;
                }

                HostScreen.Router.NavigateBack.Execute();
            }
            catch (MoneyFoxInvalidEndDateException)
            {
                await dialogService.ShowMessage(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
        }
    }
}
