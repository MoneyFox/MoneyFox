using System;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Dialogs;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross;
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
        public PaymentListPage()
        {
            InitializeComponent();

            PaymentList.ItemTapped += (sender, args) =>
            {
                PaymentList.SelectedItem = null;
                //ViewModel.EditPaymentCommand.Execute(args.Item);
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

        private async void OpenDialog()
        {
            if (Mvx.IoCProvider.CanResolve<SelectFilterDialogViewModel>())
            {
                await Navigation.PushPopupAsync(new FilterPopup
                {
                    BindingContext = Mvx.IoCProvider.Resolve<SelectFilterDialogViewModel>()
                });
            }
        }

        protected override void OnAppearing()
        {
            //Title = ViewModel.Title;

            base.OnAppearing();
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushPopupAsync(new AddPaymentPopup { BindingContext = ViewModel.RouteableViewActionRouteableViewModel });
        }

        private void EditPayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;            
            //ViewModel.EditPaymentCommand.ExecuteAsync(menuItem.CommandParameter as PaymentViewModel);
        }

        private void DeletePayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;
            //ViewModel.DeletePaymentCommand.ExecuteAsync(menuItem.CommandParameter as PaymentViewModel);
        }
    }
}