using System;
using System.Linq;
using MoneyFox.Business.ViewModels;
using MoneyFox.Dialogs;
using MoneyFox.Foundation.Resources;
using MvvmCross;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
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

        private async void OpenDialog()
        {
            await Navigation.PushPopupAsync(new FilterDialog{BindingContext = Mvx.Resolve<SelectFilterDialogViewModel>()});
        }

        protected override void OnAppearing()
        {
            Title = ViewModel.Title;

            var group = ViewModel.DailyList.FirstOrDefault(x => x.Any(y => y.IsCleared));

            if (group != null)
            {
                PaymentList.ScrollTo(group[0], group, ScrollToPosition.MakeVisible, false);
            }

            base.OnAppearing();
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(Strings.AddTitle, 
                                                  Strings.CancelLabel, 
                                                  null, 
                                                  Strings.AddExpenseLabel,
                                                  Strings.AddIncomeLabel,
                                                  Strings.AddTransferLabel);

            if (action == Strings.AddExpenseLabel)
            {
                await ViewModel.ViewActionViewModel.GoToAddExpenseCommand.ExecuteAsync();
            }
            else if (action == Strings.AddIncomeLabel)
            {
                await ViewModel.ViewActionViewModel.GoToAddIncomeCommand.ExecuteAsync();
            }
            else if (action == Strings.AddTransferLabel)
            {
                await ViewModel.ViewActionViewModel.GoToAddTransferCommand.ExecuteAsync();
            }
        }

        private void EditPayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;
            ViewModel.EditPaymentCommand.ExecuteAsync(menuItem.CommandParameter as PaymentViewModel);
        }

        private void DeletePayment(object sender, EventArgs e)
        {
            if (!(sender is MenuItem menuItem)) return;
            ViewModel.DeletePaymentCommand.ExecuteAsync(menuItem.CommandParameter as PaymentViewModel);
        }
    }
}