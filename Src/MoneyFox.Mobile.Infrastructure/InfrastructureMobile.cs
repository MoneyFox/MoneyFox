using Autofac;
using System;

namespace MoneyFox.Mobile.Infrastructure
{
    public class InfrastructureMobile : Module
    {
        protected override void Load(ContainerBuilder builder) =>
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Adapter", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces()
                   .SingleInstance();
    }
}