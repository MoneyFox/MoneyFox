using Autofac;

namespace MoneyFox.iOS
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}