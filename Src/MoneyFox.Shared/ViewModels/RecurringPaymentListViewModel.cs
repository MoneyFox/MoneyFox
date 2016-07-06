using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.ViewModels
{
    public class RecurringPaymentListViewModel : BaseViewModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDialogService dialogService;
        private readonly IPaymentManager paymentManager;

        public RecurringPaymentListViewModel(IUnitOfWork unitOfWork, IPaymentManager paymentManager, IDialogService dialogService)
        {
            this.unitOfWork = unitOfWork;
            this.paymentManager = paymentManager;
            this.dialogService = dialogService;

            AllPayments = new ObservableCollection<Payment>();
        }

        public ObservableCollection<Payment> AllPayments { get; }

        /// <summary>
        ///     Returns groupped related payments
        /// </summary>
        public ObservableCollection<AlphaGroupListGroup<Payment>> Source { get; private set; }

        /// <summary>
        ///     Prepares the recurring payment list view
        /// </summary>
        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Edits the currently selected payment.
        /// </summary>
        public MvxCommand<Payment> EditCommand { get; private set; }

        /// <summary>
        ///     Deletes the selected payment
        /// </summary>
        public MvxCommand<Payment> DeleteCommand => new MvxCommand<Payment>(Delete);

        private void Loaded()
        {
            EditCommand = null;

            AllPayments.Clear();
            foreach (var payment in paymentManager.LoadRecurringPaymentList())
            {
                AllPayments.Add(payment);
            }

            Source = new ObservableCollection<AlphaGroupListGroup<Payment>>(
                AlphaGroupListGroup<Payment>.CreateGroups(AllPayments,
                    CultureInfo.CurrentUICulture,
                    s => s.ChargedAccount.Name));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new MvxCommand<Payment>(Edit);
        }

        private void Edit(Payment payment)
        {
            unitOfWork.PaymentRepository.Selected = payment;

            ShowViewModel<ModifyPaymentViewModel>(
                new {isEdit = true, typeString = payment.Type.ToString()});
        }

        private async void Delete(Payment payment)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentConfirmationMessage))
            {
                return;
            }

            unitOfWork.PaymentRepository.Delete(payment);
            LoadedCommand.Execute();
        }
    }
}