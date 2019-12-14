using MoneyFox.Application.Common;
using MoneyFox.iOS.Renderer;
using MoneyFox.Presentation;
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
            if (Control != null)
            {
                if (ThemeManager.CurrentTheme() == AppTheme.Dark)
                {
                    UISearchBar searchBar = Control;
                    searchBar.BarStyle = UIBarStyle.Black;

                    var color = (Color) Xamarin.Forms.Application.Current.Resources["FrameHighlightColor"];
                    searchBar.BackgroundColor = color.ToUIColor();
                    searchBar.BarTintColor = color.ToUIColor();
                }
            }
        }
    }
}
