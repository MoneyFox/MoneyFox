using System;
using System.Globalization;
using Autofac;
using MediatR;
using Microsoft.Identity.Client;
using MoneyFox.Application;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.Persistence;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<PersistenceModule>();

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
                                             {
                                                 var c = context.Resolve<IComponentContext>();

                                                 return t => c.Resolve(t);
                                             });

            builder.RegisterAssemblyTypes(typeof(GetPaymentByIdQuery).Assembly).AsImplementedInterfaces(); // via assembly scan

            builder.Register(c => PublicClientApplicationBuilder
                                 .Create(ServiceConstants.MSAL_APPLICATION_ID)
                                 .WithRedirectUri($"msal{ServiceConstants.MSAL_APPLICATION_ID}://auth")
                                 .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                                 .Build());

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => !t.Name.Equals("NavigationService", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsSelf();

            CultureHelper.CurrentCulture = CultureInfo.CreateSpecificCulture(new SettingsFacade(new SettingsAdapter()).DefaultCulture);
        }
    }
}
