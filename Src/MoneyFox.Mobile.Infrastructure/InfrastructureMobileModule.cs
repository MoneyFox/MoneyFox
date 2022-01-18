using Autofac;
using MoneyFox.Infrastructure;
using System;

namespace MoneyFox.Mobile.Infrastructure
{
    public class InfrastructureMobileModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Adapter", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}