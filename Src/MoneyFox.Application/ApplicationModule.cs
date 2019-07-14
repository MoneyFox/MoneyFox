using Autofac;
using MediatR;
using System.Reflection;
using MoneyFox.Application.Infrastructure;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using Module = Autofac.Module;

namespace MoneyFox.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.RegisterInstance(AutoMapperFactory.Create());

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(GetCashFlowQuery).GetTypeInfo().Assembly).AsImplementedInterfaces();
        }
    }
}
