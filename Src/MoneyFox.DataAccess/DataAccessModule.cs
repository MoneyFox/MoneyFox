using System;
using Autofac;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Registers the dependencies for the data access module
    /// </summary>
    public class DataAccessModule : Module
    {
        /// <summary>
        ///     Registers the dependencies for the data access module
        /// </summary>
        /// <param name="builder">Containerbuilder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase))
                .AsImplementedInterfaces();

            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>();
            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>();
        }
    }
}
