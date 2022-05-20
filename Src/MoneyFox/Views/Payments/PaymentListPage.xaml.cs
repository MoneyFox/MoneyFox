namespace MoneyFox.Views.Payments
{
    using System;
    using CommunityToolkit.Maui.Views;
    using MoneyFox.Views.Dialogs;
    using ViewModels.Payments;

    [QueryProperty(name: "AccountId", queryId: "accountId")]
    public partial class PaymentListPage : ContentPage
    {
        public PaymentListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentListViewModel;
        }

        private PaymentListViewModel ViewModel => (PaymentListViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            await ViewModel.OnAppearingAsync(accountId);
        }
        private int accountId;
        public string AccountId
        {
            set => accountId = Convert.ToInt32(Uri.UnescapeDataString(value));
        }

        private void ShowFilter(object sender, EventArgs e)
        {
            this.ShowPopup(new FilterPopup());
        }
    }

}
