using Autofac;
using MoneyFox.Infrastructure;
using System;

namespace MoneyFox.Desktop.Infrastructure
{
    public class InfrastructureDesktopModule : Module
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