using System;
using Foundation;
using MoneyFox.Shared.Model;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace MoneyFox.Ios.Views.AccountList
{
	public partial class AccountTableCell : MvxTableViewCell
	{
		public static readonly NSString Key = new NSString("AccountTableCell");
		public static readonly UINib Nib = UINib.FromName("AccountTableCell", NSBundle.MainBundle);

		protected AccountTableCell(IntPtr handle) : base(handle)
		{
            this.DelayBind(() => {
                var set = this.CreateBindingSet<AccountTableCell, Account>();
                set.Bind(LabelAccountName).To(a => a.Name);
                set.Bind(LabelCurrentBalance).To(a => a.CurrentBalance).WithConversion("AmountFormat");
                set.Bind(LabelIban).To(a => a.Iban);
                set.Apply();
            });
		}
	}
}
