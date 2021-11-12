using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Infrastructure.DbBackup;
using MoneyFox.Infrastructure.Persistence;

namespace MoneyFox.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => EfCoreContextFactory.Create())
                   .As<DbContext>()
                   .AsImplementedInterfaces();

            builder.RegisterType<ContextAdapter>().AsImplementedInterfaces();
            
            builder.Register(c => PublicClientApplicationBuilder
                    .Create(AppConstants.MSAL_APPLICATION_ID)
                    .WithRedirectUri($"msal{AppConstants.MSAL_APPLICATION_ID}://auth")
                    .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                    .Build())
                .AsImplementedInterfaces();
            builder.RegisterType<OneDriveService>().AsImplementedInterfaces();
        }
    }
}
