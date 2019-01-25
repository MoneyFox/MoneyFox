using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public AddPaymentViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IMvxMessenger messenger, 
            IBackupManager backupManager,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) 
            : base(crudServices, dialogService, settingsFacade, messenger, backupManager, logProvider, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
        }

        public override void Prepare()
        {
            SelectedPayment = new PaymentViewModel();
        }

        protected override async Task SavePayment()
        {
            await crudServices.CreateAndSaveAsync(SelectedPayment, "ctor(2)");
            if (!crudServices.IsValid)
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());

            await NavigationService.Close(this);
        }
    }
}