using System;
using Autofac;
using MoneyFox.DataAccess.Infrastructure;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Used for register the dependency based in the DAL.
    /// </summary>
    public class DataAccessModule : Module
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
