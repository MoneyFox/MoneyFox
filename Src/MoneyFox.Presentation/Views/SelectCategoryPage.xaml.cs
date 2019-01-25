using MoneyFox.Foundation.Resources;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectCategoryPage 
	{
		public SelectCategoryPage ()
		{
			InitializeComponent ();
		    Title = Strings.SelectCategoryTitle;
        }
	}
}