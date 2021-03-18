using MoneyFox.Ui.Shared.PaymentSorting;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using System.Collections.Generic;

namespace MoneyFox.Uwp.Src.PaymentSorting
{
    public interface IPaymentSortOperation
    {
        IEnumerable<PaymentViewModel> Sort(ICollection<PaymentViewModel> paymentList, SortDirection sortDirection);
    }
}
