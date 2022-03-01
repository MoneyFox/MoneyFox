using MoneyFox.iOS.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]

namespace MoneyFox.iOS.Renderer
{
    using UIKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

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

            if(Application.Current.UserAppTheme == OSAppTheme.Dark)
            {
                Application.Current.Resources.TryGetValue(
                    "BackgroundColorSearchBarDark",
                    out object darkTintColor);
                searchBar.BarTintColor = ((Color)darkTintColor).ToUIColor();
            }
            else
            {
                searchBar.BarStyle = UIBarStyle.Default;

                Application.Current.Resources.TryGetValue(
                    "BackgroundColorSearchBarLight",
                    out object lightTintColor);
                searchBar.BarTintColor = ((Color)lightTintColor).ToUIColor();
            }

            Application.Current.Resources.TryGetValue("ThemePrimary", out object primaryColor);
            searchBar.TintColor = ((Color)primaryColor).ToUIColor();
            searchBar.BackgroundImage = new UIImage();
            searchBar.BackgroundColor = Color.Transparent.ToUIColor();
        }
    }
}