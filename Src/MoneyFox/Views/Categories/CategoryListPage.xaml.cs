using Xamarin.Forms;

namespace MoneyFox.Views.Categories
{
    public partial class CategoryListPage : ContentPage
    {
        public CategoryListPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.CategoryListViewModel;
        }
    }
}