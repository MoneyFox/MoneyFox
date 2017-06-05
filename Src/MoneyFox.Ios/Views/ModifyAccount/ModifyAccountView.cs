using MoneyFox.Business.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace MoneyFox.Ios.Views.ModifyAccount
{
    // TODO: refactor for the new attributes with MvvmCross 5.0
    //[MvxPanelPresentation(MvxPanelEnum.Center, MvxPanelHintType.ResetRoot, true)]
    public partial class ModifyAccountView : MvxViewController<ModifyAccountViewModel> 
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.


			this.CreateBinding(textFieldAccountName).To((ModifyAccountViewModel vm) => vm.SelectedAccount.Name).Apply();

			var cancelBtn = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, args) => ViewModel.CancelCommand.Execute());
			var saveBtn = new UIBarButtonItem(UIBarButtonSystemItem.Save, (o, args) => ViewModel.SaveCommand.Execute());

			NavigationItem.SetLeftBarButtonItem(cancelBtn, true);
			NavigationItem.SetRightBarButtonItem(saveBtn, true);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

