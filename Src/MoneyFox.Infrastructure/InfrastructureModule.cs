using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoneyFox.Infrastructure.DbBackup;
using MoneyFox.Infrastructure.Persistence;

namespace MoneyFox.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => EfCoreContextFactory.Create())
                   .As<DbContext>()
                   .AsImplementedInterfaces();

            builder.RegisterType<ContextAdapter>().AsImplementedInterfaces();

            builder.Register(
                       c => PublicClientApplicationBuilder
                            .Create(MSAL_APPLICATION_ID)
                            .WithRedirectUri($"msal{MSAL_APPLICATION_ID}://auth")
                            .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                            .Build())
                   .AsImplementedInterfaces();

            builder.RegisterType<BackupService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveService>().AsImplementedInterfaces();
        }
    }
}