using Cirrious.CrossCore;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Windows.Views
{
    public sealed partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<MainViewModel>();
        }
    }
}