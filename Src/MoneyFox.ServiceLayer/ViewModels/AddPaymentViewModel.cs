using GenericServices;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Plugin.Messenger;
using ReactiveUI;
using Splat;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IPaymentService paymentService;
        private readonly IDialogService dialogService;

        public AddPaymentViewModel(IScreen screen,
            IPaymentService paymentService = null,
            ICrudServicesAsync crudServices = null,
            IDialogService dialogService = null,
            ISettingsFacade settingsFacade = null,
            IMvxMessenger messenger = null,
            IBackupService backupService = null)
            : base(screen, crudServices, dialogService, settingsFacade, messenger, backupService)
        {

            this.paymentService = paymentService ?? Locator.Current.GetService<IPaymentService>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
        }

        public override string UrlPathSegment => "AddPayment";

        //public override void Prepare(ModifyPaymentParameter parameter)
        //{
        //    SelectedPayment = new PaymentViewModel
        //    {
        //        Type = parameter.PaymentType
        //    };
        //    Title = PaymentTypeHelper.GetViewTitleForType(parameter.PaymentType, false);
        //    base.Prepare(parameter);
        //}

        //public override async Task Initialize()
        //{
        //    await base.Initialize();
        //    SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();

        //    if (SelectedPayment.IsTransfer)
        //    {
        //        SelectedItemChangedCommand.Execute();
        //        SelectedPayment.TargetAccount = TargetAccounts.FirstOrDefault();
        //    }
        //}

        protected override async Task SavePayment()
        {
            try
            {
                var result = await paymentService.SavePayment(SelectedPayment);

                if (!result.Success)
                {
                    await dialogService.ShowMessage(Strings.GeneralErrorTitle, result.Message);
                    return;
                }

                // await NavigationService.Close(this);
            }
            catch (MoneyFoxInvalidEndDateException)
            {
                await dialogService.ShowMessage(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
        }
    }
}