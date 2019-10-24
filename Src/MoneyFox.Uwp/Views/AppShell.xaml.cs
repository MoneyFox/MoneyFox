using Windows.UI.Xaml.Controls;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell
    {
        private WindowsShellViewModel ViewModel => DataContext as WindowsShellViewModel;

        public AppShell()
        {
            InitializeComponent();
            DataContext = new WindowsShellViewModel();
            ViewModel.Initialize(ContentFrame, NavView, KeyboardAccelerators);
        }

        public Frame MainFrame => ContentFrame;
    }
}
