namespace MoneyFox.Win
{
    using Autofac;
    using Microsoft.UI.Xaml;
    using MoneyFox.Core._Pending_.Common;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WindowsModule>();
            ViewModelLocator.RegisterServices(builder);

            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }
}
