using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views {
    public sealed partial class AboutView {
        public AboutView() {
            InitializeComponent();
            DataContext = Mvx.Resolve<AboutViewModel>();
        }
    }
}