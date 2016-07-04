// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MoneyFox.Ios
{
    [Register ("AccountViewCell")]
    partial class AccountViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelAccountName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelCurrentBalance { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelIban { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LabelAccountName != null) {
                LabelAccountName.Dispose ();
                LabelAccountName = null;
            }

            if (LabelCurrentBalance != null) {
                LabelCurrentBalance.Dispose ();
                LabelCurrentBalance = null;
            }

            if (LabelIban != null) {
                LabelIban.Dispose ();
                LabelIban = null;
            }
        }
    }
}