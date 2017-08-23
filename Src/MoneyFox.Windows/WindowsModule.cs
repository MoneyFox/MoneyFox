using Autofac;
using MoneyFox.Business;
using MoneyFox.Foundation.Interfaces;

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
            builder.RegisterModule<BusinessModule>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Manager"))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterType<ShellViewModel>().AsSelf();
            builder.RegisterType<OneDriveAuthenticator>().As<IOneDriveAuthenticator>();
            builder.RegisterType<ProtectedData>().As<IProtectedData>();
            builder.RegisterType<WindowsAppInformation>().As<IAppInformation>();
            builder.RegisterType<MarketplaceOperations>().As<IStoreOperations>();
        }
    }
}
