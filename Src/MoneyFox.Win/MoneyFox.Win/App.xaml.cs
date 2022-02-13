namespace MoneyFox.Win
{
    using Autofac;
    using Microsoft.UI.Xaml;

#if !DEBUG
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
#endif

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

            var m_window = new MainWindow();
            m_window.Activate();

#if !DEBUG
            var appConfig = new AppConfig();
            AppCenter.Start(appConfig.AppCenter.Secret, typeof(Analytics), typeof(Crashes));
#endif
        }
    }
}
