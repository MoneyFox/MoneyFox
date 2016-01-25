using System.Collections.ObjectModel;
using System.Globalization;
using MoneyManager.Core.Groups;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;

namespace MoneyManager.Core.ViewModels
{
    public class RecurringPaymentListViewModel : BaseViewModel
    {
        private readonly IPaymentRepository paymentRepository;

        public RecurringPaymentListViewModel(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
            AllPayments = new ObservableCollection<Payment>();
        }

        public ObservableCollection<Payment> AllPayments { get; private set; }

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

        private void Loaded()
        {
            EditCommand = null;

            AllPayments.Clear();
            foreach (var payment in paymentRepository.LoadRecurringList())
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

        private void Edit(Payment obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
