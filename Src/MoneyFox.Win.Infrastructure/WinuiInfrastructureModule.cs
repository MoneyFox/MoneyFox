using Autofac;
using Microsoft.Identity.Client;
using MoneyFox.Infrastructure;
using System;

namespace MoneyFox.Win.Infrastructure
{
    public class WinuiInfrastructureModule : Module
    {
        private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<InfrastructureModule>();

            builder.RegisterType<DbPathProvider>().AsImplementedInterfaces();

            var publicClientApplication = PublicClientApplicationBuilder
                .Create(MSAL_APPLICATION_ID)
                .WithRedirectUri($"msal{MSAL_APPLICATION_ID}://auth")
                .Build();
            TokenCacheHelper.EnableSerialization(publicClientApplication.UserTokenCache);

            builder.Register(c => publicClientApplication).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Adapter", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}