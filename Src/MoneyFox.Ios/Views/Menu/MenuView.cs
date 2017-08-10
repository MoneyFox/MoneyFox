using System.Collections.Generic;
using CoreGraphics;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.iOS.Views;
using UIKit;

namespace MoneyFox.Ios.Views.Menu
{
    // TODO: refactor for the new attributes with MvvmCross 5.0
	//[MvxPanelPresentation(MvxPanelEnum.Left, MvxPanelHintType.ActivePanel, false)]
	public partial class MenuView : MvxViewController<MenuViewModel>
	{
		private CGColor borderColor = new CGColor(45, 177, 128);
		private UIColor TextColor = UIColor.White, BackgroundColor = UIColor.FromRGB(14, 76, 116);

		private List<MenuModel> MenuItems;

		public MenuView()
		{
			MenuItems = new List<MenuModel>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			// setup MenuItems. We have to do this here, because ViewModel isn't initialized in the ctor.
			MenuItems.Add(new MenuModel { Title = Strings.AccountsLabel, Navigate = ViewModel.ShowAccountListCommand });

			View.BackgroundColor = BackgroundColor;
			// This is the default value of edgesForExtendedLayout
			EdgesForExtendedLayout = UIRectEdge.None;

			MenuTableView.BackgroundColor = UIColor.Clear;
			//Corner radius and color
			ProfileImage.Layer.CornerRadius = (ProfileImage.Frame.Width / 2);
			ProfileImage.Layer.BorderWidth = 1.5f;
			ProfileImage.Layer.BorderColor = borderColor;
			ProfileImage.Layer.MasksToBounds = true;

			//Label colors
			BigLabel.TextColor = TextColor;
			SmallLabel.TextColor = TextColor;

			MenuTableView.Source = new MenuTableViewSource(MenuItems);
			MenuTableView.SeparatorColor = UIColor.FromRGBA(187, 187, 187, 0.1f);

			MenuTableView.TableFooterView = new UIView(CGRect.Empty) { BackgroundColor = BackgroundColor };


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

