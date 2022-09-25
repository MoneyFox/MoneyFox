namespace MoneyFox.Infrastructure.InversionOfControl;

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
using Microsoft.Identity.Client;
using Persistence;

public static class InfrastructureConfig
{
    private const string MsalApplicationId = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

    public static void Register(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddSingleton<DbContextOptions>(
            sp => new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Data Source={sp.GetService<IDbPathProvider>().GetDbPath()}").Options);

        _ = serviceCollection.AddTransient<IAppDbContext, AppDbContext>();
        RegisterRepositories(serviceCollection);
        RegisterBackupServices(serviceCollection);
        RegisterIdentityClient(serviceCollection);
    }

    private static void RegisterRepositories(IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
        _ = serviceCollection.AddTransient<IBudgetRepository, BudgetRepository>();
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

    // TODO: move to platform
    private static void RegisterIdentityClient(IServiceCollection serviceCollection)
    {
        IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder.Create(MsalApplicationId)
            .WithRedirectUri($"msal{MsalApplicationId}://auth")
            .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
            .Build();

        _ = serviceCollection.AddSingleton(publicClientApplication);
    }
}
