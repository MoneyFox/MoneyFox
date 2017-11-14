using System;
using Autofac;
using MoneyFox.DataAccess;

namespace MoneyFox.Service
{
    /// <summary>
    ///     Registers the dependencies for the service module
    /// </summary>
    public class ServiceModule : Module
    {
        /// <summary>
        ///     Registers the dependencies for the service module
        /// </summary>
        /// <param name="builder">Containerbuilder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessModule>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase))
                   .AsImplementedInterfaces();
        }
    }
}
