using MoneyFox.ViewModels.Payments;
using System;
using Xamarin.Forms;

namespace MoneyFox.Views.Payments
{
    [QueryProperty("AccountId", "accountId")]
    public partial class PaymentListPage : ContentPage
    {
        public string AccountId
        {
            set
            {
                ViewModel.Init(Convert.ToInt32(Uri.UnescapeDataString(value)));
            }
        }

        private PaymentListViewModel ViewModel => BindingContext as PaymentListViewModel;

        public PaymentListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentListViewModel;
        }
    }
}