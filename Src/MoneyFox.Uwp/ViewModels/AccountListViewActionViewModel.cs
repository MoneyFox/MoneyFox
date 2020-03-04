using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccountCount;
using MoneyFox.Domain;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Uwp.ViewModels
{
    public class AccountListViewActionViewModel : ViewModelBase, IAccountListViewActionViewModel
    {
        private const int TRANSFER_MINIMUM_ACCOUNT_COUNT = 2;

        private readonly IMediator mediator;
        private readonly NavigationService navigationService;

        public AccountListViewActionViewModel(IMediator mediator,
                                              NavigationService navigationService)
        {
            this.mediator = mediator;
            this.navigationService = navigationService;
        }

        /// <inheritdoc/>
        public RelayCommand GoToAddAccountCommand
                            => new RelayCommand(() => navigationService.Navigate(ViewModelLocator.AddAccount));

        /// <inheritdoc/>
        public RelayCommand GoToAddIncomeCommand
                            => new RelayCommand(() => navigationService.Navigate(ViewModelLocator.AddPayment, PaymentType.Income));

        /// <inheritdoc/>
        public RelayCommand GoToAddExpenseCommand
                            => new RelayCommand(() => navigationService.Navigate(ViewModelLocator.AddPayment, PaymentType.Expense));

        /// <inheritdoc/>
        public RelayCommand GoToAddTransferCommand
                            => new RelayCommand(() => navigationService.Navigate(ViewModelLocator.AddPayment, PaymentType.Transfer));


        /// <summary>
        /// Indicates if the transfer option is available or if it shall be hidden.
        /// </summary>
        [SuppressMessage("Blocker Code Smell", "S4462:Calls to \"async\" methods should not be blocking",
                         Justification = "Have to use Result")]
        public bool IsTransferAvailable
                    => mediator.Send(new GetAccountCountQuery()).Result >= TRANSFER_MINIMUM_ACCOUNT_COUNT;

        /// <summary>
        /// Indicates if the button to add new income should be enabled.
        /// </summary>
        [SuppressMessage("Blocker Code Smell", "S4462:Calls to \"async\" methods should not be blocking",
                         Justification = "Have to use Result")]
        public bool IsAddIncomeAvailable
                    => mediator.Send(new GetAccountCountQuery()).Result > 0;

        /// <summary>
        /// Indicates if the button to add a new expense should be enabled.
        /// </summary>
        [SuppressMessage("Blocker Code Smell", "S4462:Calls to \"async\" methods should not be blocking",
                         Justification = "Have to use Result")]
        public bool IsAddExpenseAvailable
                    => mediator.Send(new GetAccountCountQuery()).Result > 0;
    }
}
