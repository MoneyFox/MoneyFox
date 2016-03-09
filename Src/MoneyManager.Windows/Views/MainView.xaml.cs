using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Current.GetInstance<MainViewModel>();
        }
    }
}