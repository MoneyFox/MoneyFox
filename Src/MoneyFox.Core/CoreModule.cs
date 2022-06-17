namespace MoneyFox.Core
{

    using System;
    using System.Reflection;
    using Autofac;
    using Common.Mediatr;
    using MediatR;
    using MediatR.Pipeline;
    using Module = Autofac.Module;

    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterMediatr(builder);
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith(value: "Manager", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith(value: "Service", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith(value: "Adapter", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith(value: "Facade", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith(value: "Helper", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private void RegisterMediatr(ContainerBuilder builder)
        {
            builder.RegisterType<CustomMediator>().AsImplementedInterfaces();
            Type[] mediatrOpenTypes =
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>)
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder.RegisterAssemblyTypes(ThisAssembly)
                    .AsClosedTypesOf(mediatrOpenType)

                    // when having a single class implementing several handler types
                    // this call will cause a handler to be called twice
                    // in general you should try to avoid having a class implementing for instance `IRequestHandler<,>` and `INotificationHandler<>`
                    // the other option would be to remove this call
                    // see also https://github.com/jbogard/MediatR/issues/462
                    .AsImplementedInterfaces();
            }

            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.Register<ServiceFactory>(
                ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();

                    return t => c.Resolve(t);
                });
        }
    }

}
