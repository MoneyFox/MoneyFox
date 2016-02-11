using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MoneyManager.Foundation.Groups;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class PaymentListViewModel : BaseViewModel, IPaymentListViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IDialogService dialogService;
        private readonly IPaymentRepository paymentRepository;

        public PaymentListViewModel(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IBalanceViewModel balanceViewModel,
            IDialogService dialogService)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
            this.balanceViewModel = balanceViewModel;
            this.dialogService = dialogService;

            BalanceViewModel = new PaymentListBalanceViewModel(accountRepository, paymentRepository);
        }

        /// <summary>
        ///     Loads the data for this view.
        /// </summary>
        public virtual MvxCommand LoadCommand => new MvxCommand(LoadPayments);

        public IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     Navigate to the add payment view.
        /// </summary>
        public MvxCommand<string> GoToAddPaymentCommand => new MvxCommand<string>(GoToAddPayment);

        /// <summary>
        ///     Deletes the current account and updates the balance.
        /// </summary>
        public MvxCommand DeleteAccountCommand => new MvxCommand(DeleteAccount);

        /// <summary>
        ///     Edits the currently selected payment.
        /// </summary>
        public MvxCommand<Payment> EditCommand { get; private set; }

        /// <summary>
        ///     Deletes the passed payment.
        /// </summary>
        public MvxCommand<Payment> DeletePaymentCommand => new MvxCommand<Payment>(DeletePayment);

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
            BalanceViewModel.UpdateBalanceCommand.Execute();

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
            EditCommand = new MvxCommand<Payment>(Edit);
        }

        private void GoToAddPayment(string type)
        {
            ShowViewModel<ModifyPaymentViewModel>(new {isEdit = false, typeString = type});
        }

        private async void DeleteAccount()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteAccountConfirmationMessage))
            {
                accountRepository.Delete(accountRepository.Selected);
                balanceViewModel.UpdateBalanceCommand.Execute();
                Close(this);
            }
        }

        private void Edit(Payment payment)
        {
            paymentRepository.Selected = payment;

            ShowViewModel<ModifyPaymentViewModel>(
				new {isEdit = true, typeString = payment.Type.ToString()});
        }


        private async void DeletePayment(Payment payment)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage))
                return;

            accountRepository.RemovePaymentAmount(payment);
            paymentRepository.Delete(payment);
            LoadCommand.Execute();
        }
    }
}