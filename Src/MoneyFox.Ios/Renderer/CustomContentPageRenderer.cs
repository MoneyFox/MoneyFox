using System.Collections.Generic;
using MoneyFox.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(CustomContentPageRenderer))]
namespace MoneyFox.iOS.Renderer
{
    public class CustomContentPageRenderer : PageRenderer
    {
        public new ContentPage Element
        {
            get { return (ContentPage)base.Element; }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            List<UIBarButtonItem> LeftNavList = new List<UIBarButtonItem>();
            List<UIBarButtonItem> RightNavList = new List<UIBarButtonItem>();
            List<ToolbarItem> ToolbarList = new List<ToolbarItem>();

            UINavigationItem navigationItem = this.NavigationController.TopViewController.NavigationItem;

            // Add to new list for sorting
            foreach (ToolbarItem itm in Element.ToolbarItems)
            {
                ToolbarList.Add(itm);
            }

            // Sort the list
            ToolbarList.Sort((ToolbarItem i1, ToolbarItem i2) =>
            {
                return i1.Priority > i2.Priority ? -1 : 1;
            });

            foreach (ToolbarItem itm in ToolbarList)
            {
                if (itm.Priority < 0)
                {
                    LeftNavList.Add(itm.ToUIBarButtonItem());
                }
                else
                {
                    RightNavList.Add(itm.ToUIBarButtonItem());
                }
            }
            navigationItem.SetLeftBarButtonItems(LeftNavList.ToArray(), false);
            navigationItem.SetRightBarButtonItems(RightNavList.ToArray(), false);
        }
    }
}
