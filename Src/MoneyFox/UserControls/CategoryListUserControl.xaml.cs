using System;
using MoneyFox.Business.ViewModels;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryListUserControl
	{
	    private AbstractCategoryListViewModel ViewModel => (AbstractCategoryListViewModel) BindingContext;

		public CategoryListUserControl ()
		{
			InitializeComponent ();

		    CategoryList.ItemTapped += (sender, args) =>
		    {
		        CategoryList.SelectedItem = null;
		        ViewModel.ItemClickCommand.Execute(args.Item);
		    };

		    CategoryList.On<Android>().SetIsFastScrollEnabled(true);
        }

	    private void EditCategory(object sender, EventArgs e)
	    {
	        ViewModel.EditCategoryCommand.Execute(CategoryList.SelectedItem);
	    }

	    private void DeleteCategory(object sender, EventArgs e)
	    {
	        ViewModel.DeleteCategoryCommand.Execute(CategoryList.SelectedItem);
	    }

	    private void AddCategoryClick(object sender, EventArgs e)
	    {
	        ViewModel.CreateNewCategoryCommand.Execute();
	    }
    }
}