using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Core.Constants;
using MoneyFox.Core.DatabaseModels;
using MoneyFox.Core.Helpers;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Resources;
using MoneyFox.Core.ViewModels.Models;
using MoneyManager.Core.Helpers;
using PropertyChanged;
using IDialogService = MoneyFox.Core.Interfaces.IDialogService;

namespace MoneyFox.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class ModifyPaymentViewModel : ViewModelBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly IDefaultManager defaultManager;
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        private readonly IPaymentManager paymentManager;
        private readonly IPaymentRepository paymentRepository;

        // This has to be static in order to keep the value even if you leave the page to select a category.
        private double amount;

        public ModifyPaymentViewModel(IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IDialogService dialogService,
            IPaymentManager paymentManager,
            IDefaultManager defaultManager,
            INavigationService navigationService)
        {
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;
            this.paymentManager = paymentManager;
            this.defaultManager = defaultManager;
            this.navigationService = navigationService;
            this.accountRepository = accountRepository;

            MessengerInstance.Register<Category>(this, category => SelectedPayment.Category = category);
        }

        /// <summary>
        ///     Saves the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public RelayCommand SaveCommand => new RelayCommand(Save);

        /// <summary>
        ///     Opens to the SelectCategoryView
        /// </summary>
        public RelayCommand GoToSelectCategorydialogCommand => new RelayCommand(OpenSelectCategoryList);

        /// <summary>
        ///     Delets the PaymentViewModel or updates the existing depending on the IsEdit Flag.
        /// </summary>
        public RelayCommand DeleteCommand => new RelayCommand(Delete);

        /// <summary>
        ///     Cancels the operations.
        /// </summary>
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        /// <summary>
        ///     Resets the category of the currently selected PaymentViewModel
        /// </summary>
        public RelayCommand ResetCategoryCommand => new RelayCommand(ResetSelection);


        /// <summary>
        ///     Indicates if the view is in Edit mode.
        /// </summary>
        public bool IsEdit { get; private set; }

        /// <summary>
        ///     Indicates if the PaymentViewModel is a transfer.
        /// </summary>
        public bool IsTransfer { get; private set; }

        /// <summary>
        ///     Indicates if the reminder is endless
        /// </summary>
        public bool IsEndless { get; set; }

        /// <summary>
        ///     The Enddate for recurring PaymentViewModel
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     The selected recurrence
        /// </summary>
        public int Recurrence { get; set; }

        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        public string AmountString
        {
            get { return Utilities.FormatLargeNumbers(amount); }
            set
            {
                double convertedValue;
                if (double.TryParse(value, out convertedValue))
                {
                    amount = convertedValue;
                }
            }
        }

        /// <summary>
        ///     List with the different recurrence types.
        /// </summary>
        public List<string> RecurrenceList => new List<string>
        {
            Strings.DailyLabel,
            Strings.DailyWithoutWeekendLabel,
            Strings.WeeklyLabel,
            Strings.MonthlyLabel,
            Strings.YearlyLabel,
            Strings.BiweeklyLabel
        };

        /// <summary>
        ///     The selected PaymentViewModel
        /// </summary>
        public PaymentViewModel SelectedPayment
        {
            get { return paymentRepository.Selected; }
            set { paymentRepository.Selected = value; }
        }

        /// <summary>
        ///     Gives access to all accounts
        /// </summary>
        public ObservableCollection<Account> AllAccounts => accountRepository.Data;

        /// <summary>
        ///     Returns the Title for the page
        /// </summary>
        public string Title => PaymentViewModelTypeHelper.GetViewTitleForType(SelectedPayment.Type, IsEdit);

        /// <summary>
        ///     Returns the Header for the account field
        /// </summary>
        public string AccountHeader
            => SelectedPayment?.Type == PaymentType.Income
                ? Strings.TargetAccountLabel
                : Strings.ChargedAccountLabel;

        /// <summary>
        ///     The PaymentViewModel date
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (!IsEdit && SelectedPayment.Date == DateTime.MinValue)
                {
                    SelectedPayment.Date = DateTime.Now;
                }
                return SelectedPayment.Date;
            }
            set { SelectedPayment.Date = value; }
        }

        private Account AccountBeforeEdit { get; set; }

        /// <summary>
        ///     Init the view. Is executed after the constructor call
        /// </summary>
        /// <param name="type">Type of the PaymentViewModel.</param>
        /// <param name="isEdit">Weather the PaymentViewModel is in edit mode or not.</param>
        public void Init(PaymentType type, bool isEdit = false)
        {
            IsEdit = isEdit;
            IsEndless = true;

            amount = 0;

            if (IsEdit)
            {
                PrepareEdit();
            }
            else
            {
                PrepareDefault(type);
            }

            AccountBeforeEdit = SelectedPayment.ChargedAccount;
        }

        private void PrepareEdit()
        {
            IsTransfer = SelectedPayment.IsTransfer;
            // set the private amount property. This will get properly formatted and then displayed.
            amount = SelectedPayment.Amount;
            Recurrence = SelectedPayment.IsRecurring
                ? SelectedPayment.RecurringPayment.Recurrence
                : 0;
            EndDate = SelectedPayment.IsRecurring
                ? SelectedPayment.RecurringPayment.EndDate
                : DateTime.Now;
            IsEndless = !SelectedPayment.IsRecurring || SelectedPayment.RecurringPayment.IsEndless;
        }

        private void PrepareDefault(PaymentType type)
        {
            SetDefaultPaymentViewModel(type);
            SelectedPayment.ChargedAccount = defaultManager.GetDefaultAccount();
            IsTransfer = type == PaymentType.Transfer;
            EndDate = DateTime.Now;
        }

        private void SetDefaultPaymentViewModel(PaymentType paymentType)
        {
            SelectedPayment = new PaymentViewModel()
            {
                Type = paymentType,
                Date = DateTime.Now,
                // Assign empty category to reset the GUI
                Category = new Category()
            };
        }

        private async void Save()
        {
            if (SelectedPayment.ChargedAccount == null)
            {
                ShowAccountRequiredMessage();
                return;
            }

            if (SelectedPayment.IsRecurring && !IsEndless && EndDate.Date <= DateTime.Today)
            {
                ShowInvalidEndDateMessage();
                return;
            }

            // Make sure that the old amount is removed to not count the amount twice.
            RemoveOldAmount();
            SelectedPayment.Amount = amount;

            //Create a recurring PaymentViewModel based on the PaymentViewModel or update an existing
            await PrepareRecurringPayment();

            // Save item or update the PaymentViewModel and add the amount to the account
            paymentRepository.Save(SelectedPayment);
            accountRepository.AddPaymentAmount(SelectedPayment);

            navigationService.GoBack();
        }

        private void RemoveOldAmount()
        {
            if (IsEdit)
            {
                accountRepository.RemovePaymentAmount(SelectedPayment, AccountBeforeEdit);
            }
        }

        private async Task PrepareRecurringPayment()
        {
            if ((IsEdit && await paymentManager.CheckForRecurringPayment(SelectedPayment))
                || SelectedPayment.IsRecurring)
            {
                SelectedPayment.RecurringPayment = RecurringPaymentHelper.
                    GetRecurringFromPaymentViewModel(SelectedPayment,
                        IsEndless,
                        (PaymentRecurrence) Enum.ToObject(typeof (PaymentRecurrence), Recurrence),
                        EndDate)
                    .GetRecurringPayment();
            }
        }

        private void OpenSelectCategoryList()
        {
            navigationService.NavigateTo(NavigationConstants.SELECT_CATEGORY_LIST_VIEW);
        }

        private async void Delete()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentViewModelConfirmationMessage))
            {
                if (await paymentManager.CheckForRecurringPayment(SelectedPayment))
                {
                    paymentRepository.DeleteRecurring(SelectedPayment);
                }

                paymentRepository.Delete(paymentRepository.Selected);
                accountRepository.RemovePaymentAmount(SelectedPayment);
                navigationService.GoBack();
            }
        }

        private async void ShowAccountRequiredMessage()
        {
            await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle,
                Strings.AccountRequiredMessage);
        }

        private async void ShowInvalidEndDateMessage()
        {
            await dialogService.ShowMessage(Strings.InvalidEnddateTitle,
                Strings.InvalidEnddateMessage);
        }


        private void ResetSelection()
        {
            SelectedPayment.Category = null;
        }

        private void Cancel()
        {
            navigationService.GoBack();
        }
    }
}