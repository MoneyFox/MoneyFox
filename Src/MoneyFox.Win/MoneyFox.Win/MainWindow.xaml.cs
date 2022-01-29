namespace MoneyFox.Win
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using MoneyFox.Win.Pages;

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var rootFrame = Content as Frame;
            rootFrame.Navigate(typeof(ShellPage), null);
        }
    }
}
