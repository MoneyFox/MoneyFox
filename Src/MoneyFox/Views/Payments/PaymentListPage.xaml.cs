using MoneyFox.ViewModels.Payments;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Payments
{
    [QueryProperty("AccountId", "accountId")]
    public partial class PaymentListPage : ContentPage
    {
        private int accountId;
        public string AccountId
        {
            set => accountId = Convert.ToInt32(Uri.UnescapeDataString(value));
        }

        private PaymentListViewModel ViewModel => BindingContext as PaymentListViewModel;

        public PaymentListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentListViewModel;
        }

        protected override async void OnAppearing() => await ViewModel.OnAppearingAsync(accountId);
    }
}