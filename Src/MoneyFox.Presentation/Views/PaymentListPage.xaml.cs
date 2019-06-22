using System;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.Presentation.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentListPage
    {
        private PaymentListViewModel ViewModel => BindingContext as PaymentListViewModel;

        public PaymentListPage(int accountId)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentListVm;

            ViewModel.AccountId = accountId;
            Title = ViewModel.Title;

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

            ViewModel.InitializeCommand.Execute(null);
        }

        private async void OpenDialog()
        {
            await Navigation.PushPopupAsync(new FilterPopup
            {
                BindingContext = ViewModelLocator.SelectFilterDialogVm
            });
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AddPaymentPopup { BindingContext = ViewModel.ViewActionViewModel });
        }

        private void EditPayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;
            ViewModel.EditPaymentCommand.Execute(menuItem.CommandParameter as PaymentViewModel);
        }

        private void DeletePayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;
            ViewModel.DeletePaymentCommand.Execute(menuItem.CommandParameter as PaymentViewModel);
        }
    }
}