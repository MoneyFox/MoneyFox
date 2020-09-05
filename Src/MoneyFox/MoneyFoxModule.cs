using Autofac;
using Microsoft.Identity.Client;
using MoneyFox.Application;
using MoneyFox.Application.Common.Constants;
using MoneyFox.AutoMapper;
using MoneyFox.Persistence;
using MoneyFox.Ui.Shared.ViewModels.Settings;
using System;
using Module = Autofac.Module;

namespace MoneyFox
{
    public class MoneyFoxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<PersistenceModule>();

            builder.RegisterInstance(AutoMapperFactory.Create());

            builder.Register(c => PublicClientApplicationBuilder
                                 .Create(AppConstants.MSAL_APPLICATION_ID)
                                 .WithRedirectUri($"msal{AppConstants.MSAL_APPLICATION_ID}://auth")
                                 .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                                 .Build());

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsSelf();

            builder.RegisterAssemblyTypes(typeof(SettingsViewModel).Assembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsSelf();
        }
    }
}
