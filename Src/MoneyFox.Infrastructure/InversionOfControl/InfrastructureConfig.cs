namespace MoneyFox.Infrastructure.InversionOfControl
{

    using System;
    using Core._Pending_.Common.Facades;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Core.ApplicationCore.UseCases.BackupUpload;
    using Core.ApplicationCore.UseCases.DbBackup;
    using Core.Common.Interfaces;
    using Core.Common.Mediatr;
    using Core.Interfaces;
    using DataAccess;
    using DbBackup;
    using DbBackup.Legacy;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class InfrastructureConfig
    {
        public static void Register(ServiceCollection serviceCollection)
        {
            // TODO should this be a singleton? Would that work with the backup restore?
            serviceCollection.AddTransient<IAppDbContext>(
                sp => EfCoreContextFactory.Create(
                    publisher: sp.GetService<ICustomPublisher>() ?? throw new InvalidOperationException(),
                    settingsFacade: sp.GetService<ISettingsFacade>() ?? throw new InvalidOperationException(),
                    dbPath: sp.GetService<IDbPathProvider>()?.GetDbPath() ?? throw new InvalidOperationException()));

            serviceCollection.AddTransient<IContextAdapter, ContextAdapter>();
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
