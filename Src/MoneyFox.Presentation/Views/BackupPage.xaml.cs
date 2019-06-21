using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BackupPage
    {
        public BackupViewModel ViewModel => BindingContext as BackupViewModel;

		public BackupPage ()
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.BackupVm;

            Title = Strings.BackupTitle;

            ViewModel.InitializeCommand.Execute(null);
		}
    }
}