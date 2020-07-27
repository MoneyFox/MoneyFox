using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Category;
using Xamarin.Forms;

namespace MoneyFox.Views.Category
{
    public partial class EditCategoryPage : ContentPage
    {
        private EditCategoryViewModel ViewModel => BindingContext as EditCategoryViewModel;

        public EditCategoryPage(int categoryId)
        {
            InitializeComponent();

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            var saveItem = new ToolbarItem
            {
                Command = new Command(() => ViewModel.SaveCommand.Execute(null)),
                Text = Strings.SaveLabel,
                Priority = 1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
            ToolbarItems.Add(saveItem);
        }
    }
}