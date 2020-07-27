using Xamarin.Forms;

namespace MoneyFox.Views.Category
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