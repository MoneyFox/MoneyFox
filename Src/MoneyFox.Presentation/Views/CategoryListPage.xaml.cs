using MoneyFox.Foundation.Resources;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryListPage
	{
		public CategoryListPage()
		{
	        InitializeComponent();
		    Title = Strings.CategoriesTitle;
		}
    }
}