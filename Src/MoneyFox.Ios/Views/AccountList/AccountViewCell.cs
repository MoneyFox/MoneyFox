using System;
using Foundation;
using MoneyFox.Shared.Model;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace MoneyFox.Ios.Views.AccountList
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
            this.DelayBind(() => {
                var set = this.CreateBindingSet<AccountViewCell, Account>();
                set.Bind(LabelAccountName).To(a => a.Name);
                set.Bind(LabelCurrentBalance).To(a => a.CurrentBalance).WithConversion("AmountFormat");
                set.Bind(LabelIban).To(a => a.Iban);
                set.Apply();
            });
		}
	}
}
