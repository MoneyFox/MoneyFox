using System;
using MoneyFox.Foundation.Resources;
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
            throw new NotImplementedException();
        }

        private void DeletePayment(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}