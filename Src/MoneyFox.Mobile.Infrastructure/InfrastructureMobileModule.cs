using Autofac;
using Microsoft.Identity.Client;
using MoneyFox.Infrastructure;
using System;

namespace MoneyFox.Mobile.Infrastructure
{
    public class InfrastructureMobileModule : Module
    {
        private const string MSAL_APPLICATION_ID = "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<InfrastructureModule>();

            var publicClientApplication = PublicClientApplicationBuilder
                        .Create(MSAL_APPLICATION_ID)
                        .WithRedirectUri($"msal{MSAL_APPLICATION_ID}://auth")
                        .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                        .Build();
            builder.Register(c => publicClientApplication).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Adapter", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}