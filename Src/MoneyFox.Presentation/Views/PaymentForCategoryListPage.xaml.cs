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

        public PaymentForCategoryListPage(int categoryId, DateTime timeRangeFrom, DateTime timeRangeTo)
        {
            InitializeComponent();

            ViewModel.CategoryId = categoryId;
            ViewModel.TimeRangeFrom = timeRangeFrom;
            ViewModel.TimeRangeTo = timeRangeTo;
        }

        protected override void OnAppearing()
        {
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }
    }
}