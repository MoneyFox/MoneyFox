#nullable enable
using MoneyFox.Uwp.ViewModels.About;

namespace MoneyFox.Uwp.Views.Settings
{
    public sealed partial class AboutView
    {
        private AboutViewModel ViewModel => (AboutViewModel)DataContext;

        public AboutView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AboutVm;
        }
    }
}