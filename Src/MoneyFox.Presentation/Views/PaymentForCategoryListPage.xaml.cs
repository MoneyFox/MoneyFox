using MoneyFox.Application.Common;
using MoneyFox.Presentation.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentForCategoryListPage : ContentPage
    {
        private PaymentForCategoryListViewModel ViewModel => BindingContext as PaymentForCategoryListViewModel;

        public PaymentForCategoryListPage(PaymentForCategoryParameter parameter)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentForCategoryListVm;

            ViewModel.CategoryId = parameter.CategoryId;
            ViewModel.TimeRangeFrom = parameter.TimeRangeFrom;
            ViewModel.TimeRangeTo = parameter.TimeRangeTo;
        }

        protected override void OnAppearing()
        {
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        public class PaymentForCategoryParameter
        {
            public int CategoryId { get; set; }
            public DateTime TimeRangeFrom { get; set; }
            public DateTime TimeRangeTo { get; set; }
        }
    }
}