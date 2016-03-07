using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<AboutViewModel>();
        }
    }
}