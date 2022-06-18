namespace MoneyFox.Infrastructure.InversionOfControl
{

    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Core.ApplicationCore.UseCases.BackupUpload;
    using Core.ApplicationCore.UseCases.DbBackup;
    using Core.Common.Interfaces;
    using DataAccess;
    using DbBackup;
    using DbBackup.Legacy;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class InfrastructureConfig
    {
        public static void Register(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAppDbContext, AppDbContext>();

            RegisterRepositories(serviceCollection);
            RegisterBackupServices(serviceCollection);
        }

        private static void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
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
