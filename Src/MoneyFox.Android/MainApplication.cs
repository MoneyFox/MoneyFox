namespace MoneyFox.Droid
{

    using System;
    using System.IO;
    using Android.App;
    using Android.Runtime;
    using Autofac;
    using Core.Common;
    using Core.Common.Interfaces;
    using Core.Interfaces;
    using Infrastructure.DbBackup;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Xamarin.Essentials;

    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(javaReference: handle, transfer: transfer) { }

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
            Log.Fatal(exception: e.Exception, messageTemplate: "Application Terminating");
        }

        private static void RegisterServices()
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
                    path: Path.Combine(path1: FileSystem.AppDataDirectory, path2: LogConfiguration.FileName),
                    rollingInterval: RollingInterval.Month,
                    retainedFileCountLimit: 6,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            Log.Information("Application Startup");
        }
    }

}
