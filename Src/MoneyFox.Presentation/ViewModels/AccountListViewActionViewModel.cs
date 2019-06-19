using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Foundation;
using MoneyFox.Presentation.Parameters;
using MoneyFox.Presentation.ViewModels.Interfaces;

namespace MoneyFox.Presentation.ViewModels
{
    public class AccountListViewActionViewModel : BaseViewModel, IAccountListViewActionViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly INavigationService navigationService;

        public AccountListViewActionViewModel(ICrudServicesAsync crudServices,
                                              INavigationService navigationService)
        {
            this.crudServices = crudServices;
            this.navigationService = navigationService;
        }
        
        /// <inheritdoc />
        public RelayCommand GoToAddAccountCommand =>
                new RelayCommand(() => navigationService.NavigateTo(ViewModelLocator.AddAccount));

        /// <inheritdoc />
        public RelayCommand GoToAddIncomeCommand =>
                new RelayCommand(() => navigationService.NavigateTo(ViewModelLocator.AddPayment, new ModifyPaymentParameter(PaymentType.Income)));

        /// <inheritdoc />
        public RelayCommand GoToAddExpenseCommand =>
            new RelayCommand(() => navigationService.NavigateTo(ViewModelLocator.AddPayment, new ModifyPaymentParameter(PaymentType.Expense)));

        /// <inheritdoc />
        public RelayCommand GoToAddTransferCommand =>
            new RelayCommand(() => navigationService.NavigateTo(ViewModelLocator.AddPayment, new ModifyPaymentParameter(PaymentType.Transfer)));

        /// <summary>
        ///     Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        public bool IsTransferAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Count() >= 2;

        /// <summary>
        ///     Indicates if the button to add new income should be enabled.
        /// </summary>
        public bool IsAddIncomeAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Any();

        /// <summary>
        ///     Indicates if the button to add a new expense should be enabled.
        /// </summary>
        public bool IsAddExpenseAvailable => crudServices.ReadManyNoTracked<AccountViewModel>().Any();
    }
}
