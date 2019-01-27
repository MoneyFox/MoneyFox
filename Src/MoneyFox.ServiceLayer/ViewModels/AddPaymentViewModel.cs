using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
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

        public override void Prepare()
        {
            SelectedPayment = new PaymentViewModel();
        }

        protected override async Task SavePayment()
        {
            var result = await paymentService.SavePayment(SelectedPayment);

            if(!result.Success)
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, result.Message);

            await NavigationService.Close(this);
        }
    }
}