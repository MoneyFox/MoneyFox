using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Services;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.Utilities;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IPaymentService paymentService;
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        public AddPaymentViewModel(IPaymentService paymentService,
            ICrudServices crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            INavigationService navigationService) 
            : base(crudServices, dialogService, settingsFacade, backupService, navigationService)
        {
            this.paymentService = paymentService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        public RelayCommand InitializeCommand => new RelayCommand(Initialize);
        
        public PaymentType PaymentType { get; set; }

        private async void Initialize()
        {
            SelectedPayment = new PaymentViewModel
            {
                Type = PaymentType
            };
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