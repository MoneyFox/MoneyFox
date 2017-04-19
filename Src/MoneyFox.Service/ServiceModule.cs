using System;
using Autofac;

namespace MoneyFox.Service
{
    /// <summary>
    ///     Used for register the dependency based in the SAL.
    /// </summary>
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
