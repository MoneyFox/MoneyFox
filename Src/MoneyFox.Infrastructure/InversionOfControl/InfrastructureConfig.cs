namespace MoneyFox.Infrastructure.InversionOfControl;

using Core.Common.Interfaces;
using Core.Features.BackupUpload;
using Core.Features.DbBackup;
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
        serviceCollection.AddTransient<DbContextOptions>(
            sp => new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={sp.GetService<IDbPathProvider>()!.GetDbPath()}").Options);

        serviceCollection.AddTransient<IAppDbContext, AppDbContext>();
        RegisterBackupServices(serviceCollection);
    }

    private static void RegisterBackupServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ISqliteBackupService, SqliteBackupService>();
        serviceCollection.AddTransient<IBackupUploadService, OneDriveBackupUploadService>();
        serviceCollection.AddTransient<IOneDriveAuthenticationService, OneDriveAuthenticationService>();
        serviceCollection.AddTransient<IBackupUploadService, OneDriveBackupUploadService>();
        serviceCollection.AddTransient<IBackupService, BackupService>();
        serviceCollection.AddTransient<IOneDriveBackupService, OneDriveService>();
        serviceCollection.AddTransient<IOneDriveProfileService, OneDriveProfileService>();
    }
}
