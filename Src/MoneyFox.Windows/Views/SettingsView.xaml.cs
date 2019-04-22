using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public class MySettingsView : ReactiveView<ISettingsViewModel> { }

    public sealed partial class SettingsView 
    {
        public SettingsView() 
        {
            InitializeComponent();
            this.WhenActivated(disposables => {});
        }
    }
}
