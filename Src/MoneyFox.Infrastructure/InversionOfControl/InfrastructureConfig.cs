namespace MoneyFox.Infrastructure.InversionOfControl;

using System;
using System.IO;
using Core.Common.Interfaces;
using Core.Features;
using Core.Features.BackupUpload;
using Core.Features.DbBackup;
using DbBackup;
using DbBackup.Legacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

public static class InfrastructureConfig
{
    private const string DATABASE_NAME = "moneyfox3.db";

    public static void Register(IServiceCollection serviceCollection)
    {
        var dbPath = Path.Combine(path1: Environment.GetFolderPath(Environment.SpecialFolder.Personal), path2: DATABASE_NAME);
        serviceCollection.AddTransient<DbContextOptions>(sp => new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={dbPath}").Options);
        serviceCollection.AddTransient<IAppDbContext, AppDbContext>();
        serviceCollection.AddTransient<AppDbContext>();
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
