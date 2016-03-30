using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Core.Constants;
using MoneyFox.Core.Groups;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Interfaces.ViewModels;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;
using MoneyFox.Core.Resources;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using PropertyChanged;
using IDialogService = MoneyFox.Core.Interfaces.IDialogService;

namespace MoneyFox.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class PaymentListViewModel : ViewModelBase, IPaymentListViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;

        public PaymentListViewModel(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IBalanceViewModel balanceViewModel,
            IDialogService dialogService,
            INavigationService navigationService,
            IPaymentManager paymentManager)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
            this.balanceViewModel = balanceViewModel;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.paymentManager = paymentManager;

            BalanceViewModel = new PaymentListBalanceViewModel(accountRepository, paymentRepository);
        }

        public IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     Loads the data for this view.
        /// </summary>
        public virtual RelayCommand LoadCommand => new RelayCommand(LoadPayments);

        /// <summary>
        ///     Navigate to the add payment view.
        /// </summary>
        public RelayCommand<string> GoToAddPaymentCommand => new RelayCommand<string>(GoToAddPayment);

        /// <summary>
        ///     Deletes the current account and updates the balance.
        /// </summary>
        public RelayCommand DeleteAccountCommand => new RelayCommand(DeleteAccount);

        /// <summary>
        ///     Edits the passed payment.
        /// </summary>
        public RelayCommand<Payment> EditCommand { get; private set; }

        /// <summary>
        ///     Deletes the passed payment.
        /// </summary>
        public RelayCommand<Payment> DeletePaymentCommand => new RelayCommand<Payment>(DeletePayment);

        /// <summary>
        ///     Returns all Payment who are assigned to this repository
        ///     This has to stay until the android list with headers is implemented.
        ///     Currently only used for Android
        /// </summary>
        public ObservableCollection<Payment> RelatedPayments { get; set; }

        /// <summary>
        ///     Returns groupped related payments
        /// </summary>
        public ObservableCollection<DateListGroup<Payment>> Source { get; set; }

        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title => accountRepository.Selected.Name;

        private void LoadPayments()
        {
            EditCommand = null;
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute(null);

            RelatedPayments = new ObservableCollection<Payment>(paymentRepository
                .GetRelatedPayments(accountRepository.Selected)
                .OrderByDescending(x => x.Date)
                .ToList());

            Source = new ObservableCollection<DateListGroup<Payment>>(
                DateListGroup<Payment>.CreateGroups(RelatedPayments,
                    CultureInfo.CurrentUICulture,
                    s => s.Date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + s.Date.Year,
                    s => s.Date, true));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new RelayCommand<Payment>(Edit);
        }

        private void GoToAddPayment(string typeString)
        {
            navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW,
                Enum.Parse(typeof (PaymentType), typeString));
        }

        private async void DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                accountRepository.Delete(accountRepository.Selected);
                balanceViewModel.UpdateBalanceCommand.Execute(null);
                navigationService.GoBack();
            }
        }

        private void Edit(Payment payment)
        {
            paymentRepository.Selected = payment;

            navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW, payment);
        }

        private async void DeletePayment(Payment payment)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage))
            {
                return;
            }

            if (await paymentManager.CheckForRecurringPayment(payment))
            {
                paymentRepository.DeleteRecurring(payment);
            }

            accountRepository.RemovePaymentAmount(payment);
            paymentRepository.Delete(payment);
            LoadCommand.Execute(null);
        }
    }
}