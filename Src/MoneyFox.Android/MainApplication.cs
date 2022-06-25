namespace MoneyFox.Droid
{

    using System;
    using System.IO;
    using Core.Common;
    using global::Android.App;
    using global::Android.Runtime;
    using JetBrains.Annotations;
    using Microsoft.Maui.Storage;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;

    [Application]
    [UsedImplicitly]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(javaReference: handle, transfer: transfer) { }

        public override void OnCreate()
        {
            InitLogger();

            // Setup handler for uncaught exceptions.
            AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;
            base.OnCreate();
        }

        private void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            Log.Fatal(exception: e.Exception, messageTemplate: "Application Terminating");
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
