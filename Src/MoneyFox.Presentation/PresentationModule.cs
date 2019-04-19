using System;
using Autofac;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}