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

namespace MoneyFox.Ios.Views.AccountList
{
    [Register ("AccountListView")]
    partial class AccountListView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView AccountList { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AccountList != null) {
                AccountList.Dispose ();
                AccountList = null;
            }
        }
    }
}