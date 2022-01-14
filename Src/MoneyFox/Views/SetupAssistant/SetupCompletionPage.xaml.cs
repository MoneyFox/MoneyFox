using CommonServiceLocator;
using MoneyFox.ViewModels.SetupAssistant;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views.SetupAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupCompletionPage : ContentPage
    {
        public SetupCompletionPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<SetupCompletionViewModel>();
        }
    }
}