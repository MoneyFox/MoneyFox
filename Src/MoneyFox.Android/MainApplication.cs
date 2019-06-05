using System;
using System.IO;
using Android.App;
using Android.Runtime;
using Autofac;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.Presentation;
using NLog;
using NLog.Targets;
using Application = Android.App.Application;

namespace MoneyFox.Droid
{
    [Application]
    public class MainApplication : Application
    {
        private Logger logManager;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            InitLogger();

            logManager.Info("Application Started.");
            logManager.Info("App Version: {Version}", new DroidAppInformation().GetVersion());

            RegisterServices();
            EfCoreContext.DbPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                             DatabaseConstants.DB_NAME);

            logManager.Debug("Database Path: {dbPath}", EfCoreContext.DbPath);

            base.OnCreate();
        }

        private void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AndroidModule>();
            ViewModelLocator.RegisterServices(builder);
        }

        public override void OnTerminate()
        {
            logManager.Info("Application Terminating.");
            LogManager.Shutdown();
            base.OnTerminate();
        }

        private void InitLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new FileTarget("logfile")
            {
                FileName = Path.Combine(Path.Combine(GetExternalFilesDir(null).AbsolutePath, "moneyfox.log")),
                AutoFlush = true,
                ArchiveEvery = FileArchivePeriod.Month
            };
            var debugTarget = new DebugTarget("console");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
            logManager = LogManager.GetCurrentClassLogger();
        }
    }
}