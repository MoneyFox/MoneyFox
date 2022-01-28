namespace MoneyFox.Win
{
    using CommonServiceLocator;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using MoneyFox.Win.Services;

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var rootFrame = Content as Frame;

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Initialize(rootFrame);
        }
    }
}
