using MoneyFox.Application.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.Presentation.Views
{
    public partial class PaymentForCategoryListPage : ContentPage
    {
        public PaymentForCategoryListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentForCategoryListViewModel;

            var doneItem = new ToolbarItem
            {
                Command = new Command(async () => await Close()),
                Text = Strings.DoneLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(doneItem);
        }

        private async Task Close()
        {
            await Navigation.PopModalAsync();
        }
    }
}