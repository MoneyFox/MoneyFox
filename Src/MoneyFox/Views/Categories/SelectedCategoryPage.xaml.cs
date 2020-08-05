using MoneyFox.Application.Resources;
using Xamarin.Forms;

namespace MoneyFox.Views.Categories
{
    public partial class SelectedCategoryPage : ContentPage
    {
        public SelectedCategoryPage()
        {
            InitializeComponent();

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
        }
    }
}