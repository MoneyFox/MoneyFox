using Autofac;

namespace MoneyFox.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(AutoMapperFactory.Create());
        }
    }
}
