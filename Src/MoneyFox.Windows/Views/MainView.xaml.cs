using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.ViewModels;

namespace MoneyFox.Windows.Views
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