using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Domain.Exceptions;
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
        }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);
        
        protected override async Task Initialize()
        {
            Title = PaymentTypeHelper.GetViewTitleForType(SelectedPayment.Type, false);

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
                await paymentService.SavePayment(SelectedPayment);
                navigationService.GoBack();
            }
            catch (InvalidEndDateException)
            {
                await dialogService.ShowMessage(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
        }
    }
}