using MoneyFox.iOS.Renderer;
using Xamarin.Forms;

[assembly: ExportRenderer(handler: typeof(SearchBar), target: typeof(CustomSearchBarRenderer))]
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
            if (Control == null)
            {
                return;
            }

            var searchBar = Control;
            if (Application.Current.UserAppTheme == OSAppTheme.Dark)
            {
                Application.Current.Resources.TryGetValue(key: "BackgroundColorSearchBarDark", value: out var darkTintColor);
                searchBar.BarTintColor = ((Color)darkTintColor).ToUIColor();
            }
            else
            {
                searchBar.BarStyle = UIBarStyle.Default;
                Application.Current.Resources.TryGetValue(key: "BackgroundColorSearchBarLight", value: out var lightTintColor);
                searchBar.BarTintColor = ((Color)lightTintColor).ToUIColor();
            }

            Application.Current.Resources.TryGetValue(key: "ThemePrimary", value: out var primaryColor);
            searchBar.TintColor = ((Color)primaryColor).ToUIColor();
            searchBar.BackgroundImage = new UIImage();
            searchBar.BackgroundColor = Color.Transparent.ToUIColor();
        }
    }

}
