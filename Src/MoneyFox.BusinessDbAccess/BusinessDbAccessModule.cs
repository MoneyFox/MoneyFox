using System;
using Autofac;

namespace MoneyFox.BusinessDbAccess
{
    public class BusinessDbAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("DbAccess", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
