using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public sealed partial class SettingsView : IViewFor<ISettingsViewModel>
    {
        public SettingsView() 
        {
            this.InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ISettingsViewModel)value;
        }

        public ISettingsViewModel ViewModel { get; set; }
    }
}
