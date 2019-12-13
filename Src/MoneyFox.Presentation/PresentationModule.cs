using System;
using Autofac;
using GalaSoft.MvvmLight.Messaging;
using MediatR;
using Microsoft.Identity.Client;
using MoneyFox.Application;
using MoneyFox.Application.Constants;
using MoneyFox.Application.Payments.Queries.GetPaymentById;
using MoneyFox.BusinessLogic;
using MoneyFox.Persistence;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessLogicModule>();
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<PersistenceModule>();

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<Messenger>()
                .As<IMessenger>()
                .InstancePerLifetimeScope();

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
        }
    }
}
