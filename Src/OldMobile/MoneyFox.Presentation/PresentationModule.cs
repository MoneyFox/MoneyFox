using Autofac;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Identity.Client;
using MoneyFox.Application;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Persistence;
using MoneyFox.Presentation.AutoMapper;
using System;
using System.Globalization;
using System.Reflection;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediatr(builder);

            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<PersistenceModule>();

            builder.RegisterInstance(AutoMapperFactory.Create());

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

        private void RegisterMediatr(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach(var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(GetAccountsQuery).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}
