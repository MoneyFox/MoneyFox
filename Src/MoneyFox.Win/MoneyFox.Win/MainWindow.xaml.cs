namespace MoneyFox.Win
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using MoneyFox.Win.Pages;

    public sealed partial class MainWindow : Window
    {
        // This is a temporary fix until WinUI Dialogs are fixed
        public static Frame RootFrame { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            RootFrame = ShellFrame;
            RootFrame.Navigate(typeof(ShellPage), null);
        }
    }
}
