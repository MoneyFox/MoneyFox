namespace MoneyFox.Droid
{

    using System;
    using System.IO;
    using Autofac;
    using Core.Common;
    using global::Android.Runtime;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;

    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(javaReference: handle, transfer: transfer) { }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

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
