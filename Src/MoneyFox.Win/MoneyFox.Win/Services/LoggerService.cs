namespace MoneyFox.Win.Services;

using Core.Common;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

public static class LoggerService
{
    public static void Initialize()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.File(
                path: LogConfiguration.FileName,
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Month,
                retainedFileCountLimit: 12,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}\t[{Level:u3}]\t{Message:lj}\t{Exception}{NewLine}",
                shared: true)
            .CreateLogger();
    }
}
