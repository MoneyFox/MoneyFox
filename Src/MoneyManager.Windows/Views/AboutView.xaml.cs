using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<AboutViewModel>();
        }
    }
}