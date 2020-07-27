using Autofac;

namespace MoneyFox.Droid
{
    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}