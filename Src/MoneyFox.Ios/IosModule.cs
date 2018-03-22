using Autofac;
using MoneyFox.Business;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace MoneyFox.iOS
{
    /// <summary>
    ///     Registers the dependencies for the android project
    /// </summary>
    public class IosModule : Module
    {
        /// <summary>
        ///     Registers the dependencies for the android project
        /// </summary>
        /// <param name="builder">Containerbuilder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessModule>();

            builder.RegisterType<ConnectivityImplementation>().As<IConnectivity>();
        }
    }
}