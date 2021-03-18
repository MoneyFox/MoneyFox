using MoneyFox.Ui.Shared.PaymentSorting;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using System.Collections.Generic;
using System.Linq;

namespace MoneyFox.Uwp.Src.PaymentSorting
{
    public class PaymentSortOperationDate : IPaymentSortOperation
    {
        public IEnumerable<PaymentViewModel> Sort(ICollection<PaymentViewModel> paymentList, SortDirection sortDirection)
            => paymentList.OrderBy(x => x.Date);
    }
}
