using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.ViewModels;
using MoneyFox.Windows.Shortcuts;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views
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