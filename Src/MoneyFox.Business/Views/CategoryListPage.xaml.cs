using MoneyFox.Business.Styles;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Business.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryListPage : ContentPage
	{
		public CategoryListPage ()
		{
	        ApplyRessources();
	        InitializeComponent();

	        CategoryList.ItemTapped += (sender, args) =>
	        {
	            CategoryList.SelectedItem = null;
	            ((ICategoryListViewModel) BindingContext).ItemClickCommand.Execute(args.Item);
	        };

		    CategoryList.On<Android>().SetIsFastScrollEnabled(true);

		}

	    private void ApplyRessources()
	    {
	        if (Mvx.Resolve<ISettingsManager>().Theme == AppTheme.Dark)
	        {
	            Resources = new AppStylesDark();
	        } else
	        {
	            Resources = new AppStylesLight();
	        }

	        Resources.MergedDictionaries.Add(new AppStyles());
	    }
    }
}