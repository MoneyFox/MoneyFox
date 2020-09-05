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
            Color color;

            if(App.Current.UserAppTheme == OSAppTheme.Dark)
            {
                color = (Color)Xamarin.Forms.Application.Current.Resources["SearchBarFrameColor_Dark"];
            }
            else
            {
                color = (Color)Xamarin.Forms.Application.Current.Resources["SearchBarFrameColor_Light"];
            }

            searchBar.BackgroundColor = color.ToUIColor();
                searchBar.BarTintColor = color.ToUIColor();
        }
    }
}
