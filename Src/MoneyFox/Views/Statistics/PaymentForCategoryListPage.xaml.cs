namespace MoneyFox.Views.Statistics
{

    using System.Threading.Tasks;
    using Core.Resources;

    public partial class PaymentForCategoryListPage : ContentPage
    {
        public PaymentForCategoryListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PaymentForCategoryListViewModel;
            var doneItem = new ToolbarItem
            {
                Command = new Command(async () => await CloseAsync()),
                Text = Strings.DoneLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(doneItem);
        }

        private async Task CloseAsync()
        {
            await Navigation.PopModalAsync();
        }
    }

}
