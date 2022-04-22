namespace MoneyFox.Droid
{

    using System;
    using System.IO;
    using Android.App;
    using Android.Runtime;
    using Autofac;
    using Core.Common;
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
