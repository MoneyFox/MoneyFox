using MoneyFox.Application.Resources;
using MoneyFox.ViewModels.Categories;
using Xamarin.Forms;

namespace MoneyFox.Views.Categories
{
    public partial class SelectedCategoryPage : ContentPage
    {
        private SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;

        public SelectedCategoryPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.SelectCategoryViewModel;

            var cancelItem = new ToolbarItem
            {
                Command = new Command(async () => await Navigation.PopModalAsync()),
                Text = Strings.CancelLabel,
                Priority = -1,
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(cancelItem);
        }

        protected override async void OnAppearing() => await ViewModel.InitializeAsync();
    }
}