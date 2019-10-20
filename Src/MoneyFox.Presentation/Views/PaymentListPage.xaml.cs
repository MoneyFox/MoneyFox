using System;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MoneyFox.Presentation.Views
{
    public partial class PaymentListPage
    {
        private PaymentListViewModel ViewModel => BindingContext as PaymentListViewModel;

        public PaymentListPage(int accountId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentListVm;

            ViewModel.AccountId = accountId;

            PaymentList.ItemTapped += (sender, args) =>
            {
                PaymentList.SelectedItem = null;
                ViewModel.EditPaymentCommand.Execute(args.Item);
            };

            PaymentList.On<Android>().SetIsFastScrollEnabled(true);

            var filterItem = new ToolbarItem
            {
                Command = new Command(OpenDialog),
                Text = Strings.FilterLabel,
                Priority = 0,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(filterItem);
        }

        protected override void OnAppearing()
        {
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafe();
        }

        private void OpenDialog()
        {
            Navigation.PushPopupAsync(new FilterPopup
            {
                BindingContext = ViewModelLocator.SelectFilterDialogVm
            }).FireAndForgetSafe();
        }

        private void AddItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushPopupAsync(new AddPaymentPopup {BindingContext = ViewModel.ViewActionViewModel}).FireAndForgetSafe();
        }

        private void EditPayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;
            ViewModel.EditPaymentCommand.Execute(menuItem.CommandParameter as PaymentViewModel);
        }

        private void DeletePayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;
            ViewModel.DeletePaymentCommand.ExecuteAsync(menuItem.CommandParameter as PaymentViewModel).FireAndForgetSafe();
        }
    }
}
