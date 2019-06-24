using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.Utilities;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IPaymentService paymentService;
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        public AddPaymentViewModel(IPaymentService paymentService,
            ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            INavigationService navigationService) 
            : base(crudServices, dialogService, settingsFacade, backupService, navigationService)
        {
            this.paymentService = paymentService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;

            SelectedPayment = new PaymentViewModel
            {
                Type = PaymentType
            };
        }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);
        
        public PaymentType PaymentType { get; set; }

        protected override async Task Initialize()
        {
            
            Title = PaymentTypeHelper.GetViewTitleForType(PaymentType, false);

            await base.Initialize();

            SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();

            if (SelectedPayment.IsTransfer)
            {
                SelectedItemChangedCommand.Execute(null);
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

                navigationService.GoBack();
            }
            catch (MoneyFoxInvalidEndDateException)
            {
                await dialogService.ShowMessage(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
        }
    }
}