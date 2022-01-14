using Autofac;
using System;

namespace MoneyFox.Desktop.Infrastructure
{
    public class InfrastructureDesktop : Module
    {
        protected override void Load(ContainerBuilder builder) =>
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Adapter", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces()
                   .SingleInstance();
    }
}