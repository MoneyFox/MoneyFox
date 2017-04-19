using System;
using System.Reflection;

namespace MoneyFox.Service
{
    /// <summary>
    ///     Used for register the dependency based in the SAL.
    /// </summary>
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            MapperConfiguration.Setup();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Manager", StringComparison.OrdinalIgnoreCase))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
        }
    }
}
