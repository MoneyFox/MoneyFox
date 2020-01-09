using System;
using System.IO;
using Android.App;
using Android.Runtime;
using Autofac;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Presentation;
using NLog;
using NLog.Config;
using NLog.Targets;
using PCLAppConfig;
using PCLAppConfig.FileSystemStream;
using Xamarin.Essentials;

namespace MoneyFox.Droid
{
    [Application]
    public class MainApplication : Android.App.Application
    {
        private Logger logManager;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            if (ConfigurationManager.AppSettings == null) ConfigurationManager.Initialise(PortableStream.Current);

            InitLogger();

            logManager.Info("Application Started.");
            logManager.Info("App Version: {Version}", new DroidAppInformation().GetVersion());

            // Setup handler for uncaught exceptions.
            AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;

            RegisterServices();
            base.OnCreate();
        }

        void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            logManager.Fatal(e.Exception, "Application Terminating. 1");
        }

        private void RegisterServices()
        {
            logManager.Debug("Register Services.");

            var builder = new ContainerBuilder();
            builder.RegisterModule<AndroidModule>();

            ViewModelLocator.RegisterServices(builder);

            logManager.Debug("Register Services finished.");
        }

        public override void OnTerminate()
        {
            logManager.Info("Application Terminating.");
            LogManager.Shutdown();
            base.OnTerminate();
        }

        private void InitLogger()
        {
            var config = new LoggingConfiguration();

            var logfile = new FileTarget("logfile")
            {
                FileName = Path.Combine(FileSystem.CacheDirectory, AppConstants.LogFileName),
                AutoFlush = true,
                ArchiveEvery = FileArchivePeriod.Month
            };
            var debugTarget = new DebugTarget("console");


#if !DEBUG
            // Configure AppCenter
            var appCenterTarget = new AppCenterTarget("appcenter")
            {
                AppSecret = ConfigurationManager.AppSettings["AndroidAppcenterSecret"]
            };

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, appCenterTarget);
#endif

            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
            logManager = LogManager.GetCurrentClassLogger();
        }
    }
}
