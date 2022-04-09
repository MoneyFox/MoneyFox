namespace MoneyFox.Droid
{
    using Android.App;
    using Android.Runtime;
    using Autofac;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using System;
    using System.IO;
    using Core.Common;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Xamarin.Essentials;

    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            InitLogger();

            // Setup handler for uncaught exceptions.
            AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;

            RegisterServices();
            base.OnCreate();
        }

        private void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            Log.Fatal(e.Exception, "Application Terminating");
        }

        private void RegisterServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AndroidModule>();

            ViewModelLocator.RegisterServices(builder);
        }

        private void InitLogger()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.File(
                    path: Path.Combine(FileSystem.AppDataDirectory, LogConfiguration.FileName),
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Month,
                    retainedFileCountLimit: 12,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}\t[{Level:u3}]\t{Message:lj}\t{Exception}{NewLine}",
                    shared: true)
                .CreateLogger();

            Log.Information("Application Startup");
        }
    }
}
