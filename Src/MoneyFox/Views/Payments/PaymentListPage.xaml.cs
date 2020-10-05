using MoneyFox.ViewModels.Payments;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Payments
{
    [QueryProperty("AccountId", "accountId")]
    public partial class PaymentListPage : ContentPage
    {
#pragma warning disable S2376 // Write-only properties should not be used
        private int accountId;
        public string AccountId
        {
            set => accountId = Convert.ToInt32(Uri.UnescapeDataString(value));
        }
#pragma warning restore S2376 // Write-only properties should not be used

        private PaymentListViewModel ViewModel => (PaymentListViewModel)BindingContext;

        public PaymentListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentListViewModel;
        }

        protected override async void OnAppearing()
        {
            ViewModel.Subscribe();
            await ViewModel.OnAppearingAsync(accountId);
        }

        protected override void OnDisappearing()
        {
            ViewModel.Unsubscribe();
        }
    }
}