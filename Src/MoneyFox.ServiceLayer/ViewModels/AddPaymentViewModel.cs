using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Parameters;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IPaymentService paymentService;
        private readonly IDialogService dialogService;

        public AddPaymentViewModel(IPaymentService paymentService,
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
            this.dialogService = dialogService;
        }

        public override void Prepare(ModifyPaymentParameter parameter)
        {
            SelectedPayment = new PaymentViewModel
            {
                Type = parameter.PaymentType
            };
            Title = PaymentTypeHelper.GetViewTitleForType(parameter.PaymentType, false);
            base.Prepare(parameter);
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();

            if (SelectedPayment.IsTransfer)
            {
                SelectedItemChangedCommand.Execute();
                SelectedPayment.TargetAccount = TargetAccounts.FirstOrDefault();
            }
        }

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

                await NavigationService.Close(this);
            }
            catch (MoneyFoxInvalidEndDateException)
            {
                await dialogService.ShowMessage(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
        }
    }
}