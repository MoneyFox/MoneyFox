using MoneyFox.Controls;
using MoneyFox.iOS.Renderer;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ModalContentPage), typeof(ModalContentPageRenderer))]

namespace MoneyFox.iOS.Renderer
{
    public class ModalContentPageRenderer : PageRenderer
    {
        public new ContentPage Element => (ContentPage)base.Element;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var LeftNavList = new List<UIBarButtonItem>();
            var RightNavList = new List<UIBarButtonItem>();
            var ToolbarList = new List<ToolbarItem>();

            var navigationItem = NavigationController.TopViewController.NavigationItem;

            // Add to new list for sorting
            foreach(var itm in Element.ToolbarItems)
            {
                ToolbarList.Add(itm);
            }

            // Sort the list
            ToolbarList.Sort(
                (i1, i2) =>
                {
                    return i1.Priority > i2.Priority
                        ? -1
                        : 1;
                });

            foreach(var itm in ToolbarList)
            {
                if(itm.Priority < 0)
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