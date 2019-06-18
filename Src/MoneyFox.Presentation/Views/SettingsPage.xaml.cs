using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Settings", Icon = "ic_settings_black")]
    public partial class SettingsPage 
	{
		public SettingsPage ()
		{
			InitializeComponent ();

		    SettingsList.ItemTapped += (sender, args) =>
		    {
		        SettingsList.SelectedItem = null;
                (BindingContext as SettingsViewModel)?.GoToSettingCommand.Execute(args.Item);
		    };
        }
	}
}