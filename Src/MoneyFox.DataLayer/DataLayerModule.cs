using Autofac;

namespace MoneyFox.DataLayer
{
    public class DataLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfCoreContext>().AsSelf();
        }
    }
}
