using MoneyFox.Business.ViewModels;
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