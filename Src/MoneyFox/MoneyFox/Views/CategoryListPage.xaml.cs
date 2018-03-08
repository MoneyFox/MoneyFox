using System;
using MoneyFox.Business.ViewModels;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryListPage
	{
		public CategoryListPage()
		{
	        InitializeComponent();

	        CategoryList.ItemTapped += (sender, args) =>
	        {
	            CategoryList.SelectedItem = null;
	            ((ICategoryListViewModel) BindingContext).ItemClickCommand.Execute(args.Item);
	        };

		    CategoryList.On<Android>().SetIsFastScrollEnabled(true);
        }

	    private void EditCategory(object sender, EventArgs e)
	    {
	        throw new NotImplementedException();
	    }

	    private void DeleteCategory(object sender, EventArgs e)
	    {
	        throw new NotImplementedException();
	    }
	}
}