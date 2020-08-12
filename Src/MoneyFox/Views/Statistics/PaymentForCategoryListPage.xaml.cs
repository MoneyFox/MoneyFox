using MoneyFox.Application.Common;
using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Statistics;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class PaymentForCategoryListPage : ContentPage
    {
        private PaymentForCategoryListViewModel ViewModel => BindingContext as PaymentForCategoryListViewModel;

        public PaymentForCategoryListPage(PaymentForCategoryParameter parameter)
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentForCategoryListViewModel;

            Title = Strings.PaymentsForCategoryTitle;

            ViewModel.CategoryId = parameter.CategoryId;
            ViewModel.TimeRangeFrom = parameter.TimeRangeFrom;
            ViewModel.TimeRangeTo = parameter.TimeRangeTo;

            var doneItem = new ToolbarItem
            {
                Command = new Command(async () => await Close()),
                Text = Strings.DoneLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(doneItem);
        }

        protected override void OnAppearing()
        {
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafeAsync();
        }

        private async Task Close()
        {
            await Navigation.PopModalAsync();
        }

        public class PaymentForCategoryParameter
        {
            public int CategoryId { get; set; }
            public DateTime TimeRangeFrom { get; set; }
            public DateTime TimeRangeTo { get; set; }
        }
    }
}