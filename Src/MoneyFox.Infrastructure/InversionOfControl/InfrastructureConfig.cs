namespace MoneyFox.Infrastructure.InversionOfControl
{

    using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Core.ApplicationCore.UseCases.BackupUpload;
    using Core.ApplicationCore.UseCases.DbBackup;
    using Core.Common.Interfaces;
    using Core.Interfaces;
    using DataAccess;
    using DbBackup;
    using DbBackup.Legacy;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class InfrastructureConfig
    {
        public static void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DbContextOptions>(
                sp => new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={sp.GetService<IDbPathProvider>()?.GetDbPath()}").Options);

            serviceCollection.AddTransient<IAppDbContext, AppDbContext>();
            RegisterRepositories(serviceCollection);
            RegisterBackupServices(serviceCollection);
        }

        private static void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddTransient<IBudgetRepository, BudgetRepository>();
        }

        private static void RegisterBackupServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBackupUploadService, OneDriveBackupUploadService>();
            serviceCollection.AddTransient<IOneDriveAuthenticationService, OneDriveAuthenticationService>();
            serviceCollection.AddTransient<IBackupUploadService, OneDriveBackupUploadService>();
            serviceCollection.AddTransient<IBackupService, BackupService>();
            serviceCollection.AddTransient<IOneDriveBackupService, OneDriveService>();
        }
    }

}
