using System;

using Foundation;
using UIKit;

namespace MoneyFox.Ios
{
	public partial class AccountViewCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("AccountViewCell");
		public static readonly UINib Nib;

		static AccountViewCell()
		{
			Nib = UINib.FromName("AccountViewCell", NSBundle.MainBundle);
		}

		protected AccountViewCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}
}
