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
using MoneyFox.Core.Resources;
using MoneyManager.Core.ViewModels;
using PropertyChanged;
using IDialogService = MoneyFox.Core.Interfaces.IDialogService;

namespace MoneyFox.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class PaymentViewModelListViewModel : ViewModelBase, IPaymentViewModelListViewModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBalanceViewModel balanceViewModel;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;

        public PaymentViewModelListViewModel(IPaymentRepository paymentRepository,
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

            BalanceViewModel = new PaymentViewModelListBalanceViewModel(accountRepository, paymentRepository);
        }

        public IBalanceViewModel BalanceViewModel { get; }

        /// <summary>
        ///     Loads the data for this view.
        /// </summary>
        public virtual RelayCommand LoadCommand => new RelayCommand(LoadPaymentViewModels);

        /// <summary>
        ///     Navigate to the add PaymentViewModel view.
        /// </summary>
        public RelayCommand<string> GoToAddPaymentViewModelCommand => new RelayCommand<string>(GoToAddPaymentViewModel);

        /// <summary>
        ///     Deletes the current account and updates the balance.
        /// </summary>
        public RelayCommand DeleteAccountCommand => new RelayCommand(DeleteAccount);

        /// <summary>
        ///     Edits the passed PaymentViewModel.
        /// </summary>
        public RelayCommand<PaymentViewModel> EditCommand { get; private set; }

        /// <summary>
        ///     Deletes the passed PaymentViewModel.
        /// </summary>
        public RelayCommand<PaymentViewModel> DeletePaymentViewModelCommand => new RelayCommand<PaymentViewModel>(DeletePaymentViewModel);

        /// <summary>
        ///     Returns all PaymentViewModel who are assigned to this repository
        ///     This has to stay until the android list with headers is implemented.
        ///     Currently only used for Android
        /// </summary>
        public ObservableCollection<PaymentViewModel> RelatedPaymentViewModels { get; set; }

        /// <summary>
        ///     Returns groupped related PaymentViewModels
        /// </summary>
        public ObservableCollection<DateListGroup<PaymentViewModel>> Source { get; set; }

        /// <summary>
        ///     Returns the name of the account title for the current page
        /// </summary>
        public string Title => accountRepository.Selected.Name;

        private void LoadPaymentViewModels()
        {
            EditCommand = null;
            //Refresh balance control with the current account
            BalanceViewModel.UpdateBalanceCommand.Execute(null);

            RelatedPaymentViewModels = new ObservableCollection<PaymentViewModel>(paymentRepository
                .GetRelatedPayments(accountRepository.Selected)
                .OrderByDescending(x => x.Date)
                .ToList());

            Source = new ObservableCollection<DateListGroup<PaymentViewModel>>(
                DateListGroup<PaymentViewModel>.CreateGroups(RelatedPaymentViewModels,
                    CultureInfo.CurrentUICulture,
                    s => s.Date.ToString("MMMM", CultureInfo.InvariantCulture) + " " + s.Date.Year,
                    s => s.Date, true));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new RelayCommand<PaymentViewModel>(Edit);
        }

        private void GoToAddPaymentViewModel(string typeString)
        {
            navigationService.NavigateTo(NavigationConstants.MODIFY_PaymentViewModel_VIEW,
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

        private void Edit(PaymentViewModel paymentViewModel)
        {
            paymentRepository.Selected = paymentViewModel;

            navigationService.NavigateTo(NavigationConstants.MODIFY_PaymentViewModel_VIEW, paymentViewModel);
        }

        private async void DeletePaymentViewModel(PaymentViewModel PaymentViewModel)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentViewModelConfirmationMessage))
            {
                return;
            }

            if (await paymentManager.CheckForRecurringPayment(PaymentViewModel))
            {
                paymentRepository.DeleteRecurring(PaymentViewModel);
            }

            accountRepository.RemovePaymentAmount(PaymentViewModel);
            paymentRepository.Delete(PaymentViewModel);
            LoadCommand.Execute(null);
        }
    }
}