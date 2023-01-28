namespace MoneyFox.Infrastructure.InversionOfControl;

using Core.ApplicationCore.UseCases.BackupUpload;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Common.Interfaces;
using Core.Interfaces;
using DbBackup;
using DbBackup.Legacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

public static class InfrastructureConfig
{
    public static void Register(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<DbContextOptions>(
            sp => new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={sp.GetService<IDbPathProvider>().GetDbPath()}").Options);

        _ = serviceCollection.AddTransient<IAppDbContext, AppDbContext>();
        RegisterBackupServices(serviceCollection);
    }

    private static void RegisterBackupServices(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<IBackupUploadService, OneDriveBackupUploadService>();
        _ = serviceCollection.AddTransient<IOneDriveAuthenticationService, OneDriveAuthenticationService>();
        _ = serviceCollection.AddTransient<IBackupUploadService, OneDriveBackupUploadService>();
        _ = serviceCollection.AddTransient<IBackupService, BackupService>();
        _ = serviceCollection.AddTransient<IOneDriveBackupService, OneDriveService>();
        _ = serviceCollection.AddTransient<IOneDriveProfileService, OneDriveProfileService>();
    }
}
