using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace MoneyFox.Ios
{
	public partial class AccountViewCell : MvxTableViewCell
	{
		public static readonly NSString Key = new NSString("AccountViewCell");
		public static readonly UINib Nib;

		static AccountViewCell()
		{
			Nib = UINib.FromName("AccountViewCell", NSBundle.MainBundle);
		}

		protected AccountViewCell(IntPtr handle) : base(handle)
		{

		}
	}
}
