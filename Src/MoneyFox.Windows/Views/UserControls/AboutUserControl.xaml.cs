using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyAboutUserControl : ReactiveUserControl<AboutViewModel> { }

    public sealed partial class AboutUserControl
    {
        public AboutUserControl() 
        {
            this.InitializeComponent();
        }
    }
}
