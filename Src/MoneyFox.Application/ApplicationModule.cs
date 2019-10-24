using System.Reflection;
using Autofac;
using MediatR;
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
