using MoneyFox.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace MoneyFox.iOS.Renderer
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if(Control == null)
            {
                return;
            }

            UISearchBar searchBar = Control;

            if(App.Current.UserAppTheme == OSAppTheme.Dark)
            {
                searchBar.BarStyle = UIBarStyle.Black;

                App.Current.Resources.TryGetValue("BackgroundColorDark", out var darkTintColor);
                searchBar.BarTintColor = ((Color)darkTintColor).ToUIColor();
            }
            else
            {
                searchBar.BarStyle = UIBarStyle.Default;

                App.Current.Resources.TryGetValue("BackgroundColorLight", out var lightTintColor);
                searchBar.BarTintColor = ((Color)lightTintColor).ToUIColor();
            }

            App.Current.Resources.TryGetValue("ThemePrimary", out var primaryColor);
            searchBar.TintColor = ((Color)primaryColor).ToUIColor();
            searchBar.BackgroundImage = new UIImage();
            searchBar.BackgroundColor = Color.Transparent.ToUIColor();
        }
    }
}
