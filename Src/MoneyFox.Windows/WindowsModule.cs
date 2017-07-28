using Autofac;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Windows.Business;

namespace MoneyFox.Windows
{    
    /// <summary>
    ///     Registers the dependencies for the business module
    /// </summary>
    public class WindowsModule : Module
    {
        /// <summary>
        ///     Registers the dependencies for the windows module
        /// </summary>
        /// <param name="builder">Containerbuilder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Manager"))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>();
            builder.RegisterType<ProtectedData>().As<IProtectedData>();
            builder.RegisterType<WindowsAppInformation>().As<IAppInformation>();
            builder.RegisterType<MarketplaceOperations>().As<IStoreOperations>();
        }
    }
}
