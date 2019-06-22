using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell
    {
		public AppShell()
		{
            InitializeComponent();
            BindingContext = ViewModelLocator.ShellVm;
        }
    }
}