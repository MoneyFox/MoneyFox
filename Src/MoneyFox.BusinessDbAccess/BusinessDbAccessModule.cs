using Autofac;
using MoneyFox.DataLayer;

namespace MoneyFox.BusinessDbAccess
{
    public class BusinessDbAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataLayerModule>();
        }
    }
}
