using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;

namespace MoneyFox.Windows.Views
{
    public sealed partial class BackupView
    {
        public BackupView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<BackupViewModel>();
        }
    }
}