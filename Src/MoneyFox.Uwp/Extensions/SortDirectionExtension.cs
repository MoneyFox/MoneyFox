using Microsoft.Toolkit.Uwp.UI.Controls;
using MoneyFox.Ui.Shared.PaymentSorting;

namespace MoneyFox.Uwp.Extensions
{
    public static class SortDirectionExtension
    {
        public static DataGridSortDirection ToDataGridDirection(this SortDirection direction)
            => direction == SortDirection.Ascending
                ? DataGridSortDirection.Ascending
                : DataGridSortDirection.Descending;
    }
}
