// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MoneyFox.Ios
{
    [Register ("MenuTableViewCell")]
    partial class MenuTableViewCell
    {
        [Outlet]
        UIKit.UILabel LabelMenuItemName { get; set; }



        [Outlet]
        UIKit.UIImageView MenuItemImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LabelMenuItemName != null) {
                LabelMenuItemName.Dispose ();
                LabelMenuItemName = null;
            }

            if (MenuItemImage != null) {
                MenuItemImage.Dispose ();
                MenuItemImage = null;
            }
        }
    }
}