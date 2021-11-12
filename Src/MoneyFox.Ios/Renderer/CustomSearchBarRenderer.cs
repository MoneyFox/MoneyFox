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

            var searchBar = Control;

            if(Xamarin.Forms.Application.Current.UserAppTheme == OSAppTheme.Dark)
            {
                Xamarin.Forms.Application.Current.Resources.TryGetValue(
                    "BackgroundColorSearchBarDark",
                    out var darkTintColor);
                searchBar.BarTintColor = ((Color)darkTintColor).ToUIColor();
            }
            else
            {
                searchBar.BarStyle = UIBarStyle.Default;

                Xamarin.Forms.Application.Current.Resources.TryGetValue(
                    "BackgroundColorSearchBarLight",
                    out var lightTintColor);
                searchBar.BarTintColor = ((Color)lightTintColor).ToUIColor();
            }

            Xamarin.Forms.Application.Current.Resources.TryGetValue("ThemePrimary", out var primaryColor);
            searchBar.TintColor = ((Color)primaryColor).ToUIColor();
            searchBar.BackgroundImage = new UIImage();
            searchBar.BackgroundColor = Color.Transparent.ToUIColor();
        }
    }
}