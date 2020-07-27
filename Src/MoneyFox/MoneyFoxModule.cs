using Autofac;
using MoneyFox.Application;
using MoneyFox.Persistence;
using System;

namespace MoneyFox
{
    public class MoneyFoxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<PersistenceModule>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsSelf();
        }
    }
}
