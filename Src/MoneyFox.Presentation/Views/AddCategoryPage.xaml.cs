using MoneyFox.Foundation.Resources;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCategoryPage
    {
		public AddCategoryPage ()
		{
			InitializeComponent ();

            this.WhenActivated(disposables =>
            {
                Title = ViewModel.Title;

                ToolbarItems.Add(new ToolbarItem {
                    Command = new Command(() => ViewModel.SaveCommand.Execute()),
                    Text = Strings.SaveCategoryLabel,
                    Priority = 0,
                    Order = ToolbarItemOrder.Primary,
                    Icon = "ic_save.png"
                });
            });
        }
	}
}