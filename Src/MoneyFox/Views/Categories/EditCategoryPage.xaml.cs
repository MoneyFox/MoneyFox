using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Categories;
using Xamarin.Forms;

namespace MoneyFox.Views.Categories
{
    public partial class EditCategoryPage
    {
        private EditCategoryViewModel ViewModel => (EditCategoryViewModel) BindingContext;

        private int categoryId;

        public EditCategoryPage(int categoryId)
        {
            InitializeComponent();

            this.categoryId = categoryId;

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

        protected override async void OnAppearing() => await ViewModel.InitializeAsync(categoryId);
    }
}