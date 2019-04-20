using System;
using Autofac;
using MoneyFox.BusinessDbAccess;

namespace MoneyFox.BusinessLogic
{
    public class BusinessLogicModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessDbAccessModule>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("DataProvider", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Adapter", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Action", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Manager", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
