using MoneyFox.Foundation.Resources;

namespace MoneyFox.Presentation.Views
{
	public partial class BackgroundJobSettingsPage
	{
		public BackgroundJobSettingsPage()
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.SettingsBackgroundVm;
        }

        protected override void OnAppearing()
	    {
	        Title = Strings.BackgroundJobTitle;
            base.OnAppearing();
	    }
	}
}