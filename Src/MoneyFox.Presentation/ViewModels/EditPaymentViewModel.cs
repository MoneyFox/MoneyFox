using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Services;
using MoneyFox.ServiceLayer.Utilities;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class EditPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly IPaymentService paymentService;
        private readonly INavigationService navigationService;
        private readonly IBackupService backupService;
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;

        public EditPaymentViewModel(IPaymentService paymentService,
            ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade, 
            IBackupService backupService,
            INavigationService navigationService) 
            : base(crudServices, dialogService, settingsFacade, backupService, navigationService)
        {
            this.paymentService = paymentService;
            this.navigationService = navigationService;
            this.crudServices = crudServices;
            this.dialogService = dialogService;
            this.backupService = backupService;
            this.paymentService = paymentService;
        }

        public int PaymentId { get; set; }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public RelayCommand DeleteCommand => new RelayCommand(DeletePayment);

        public RelayCommand InitializeCommand => new RelayCommand(Initialize);

        private async void Initialize()
        {
            SelectedPayment = crudServices.ReadSingleAsync<PaymentViewModel>(PaymentId).Result;

            // We have to set this here since otherwise the end date is null. This causes a crash on android.
            // Also it's user unfriendly if you the default end date is the 1.1.0001.
            if (SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment.IsEndless)
            {
                SelectedPayment.RecurringPayment.EndDate = DateTime.Today;
            }

            Title = PaymentTypeHelper.GetViewTitleForType(SelectedPayment.Type, true);

            await base.Initialize();
        }

        protected override async Task SavePayment()
        {
            var result = await paymentService.UpdatePayment(SelectedPayment);

            if (!result.Success)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
                return;
            }

            navigationService.GoBack();
        }


        private async void DeletePayment()
        {
            await paymentService.DeletePayment(SelectedPayment);
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            CancelCommand.Execute(null);
        }
    }
}