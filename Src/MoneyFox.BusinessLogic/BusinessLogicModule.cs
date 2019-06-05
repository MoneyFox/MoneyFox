
using Autofac;
using MoneyFox.BusinessDbAccess;

namespace MoneyFox.BusinessLogic
{
    public class BusinessLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessDbAccessModule>();
        }
    }
}
