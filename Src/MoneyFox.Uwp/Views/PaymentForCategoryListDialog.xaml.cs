using MoneyFox.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class PaymentForCategoryListDialog : ContentDialog
    {
        private PaymentForCategoryListViewModel ViewModel => DataContext as PaymentForCategoryListViewModel;

        public PaymentForCategoryListDialog(PaymentForCategoryParameter parameter)
        {
            InitializeComponent();

            ViewModel.CategoryId = parameter.CategoryId;
            ViewModel.TimeRangeFrom = parameter.TimeRangeFrom;
            ViewModel.TimeRangeTo = parameter.TimeRangeTo;
        }

        public class PaymentForCategoryParameter
        {
            public int CategoryId { get; set; }
            public DateTime TimeRangeFrom { get; set; }
            public DateTime TimeRangeTo { get; set; }
        }
    }
}
