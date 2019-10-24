using Autofac;
using Microsoft.EntityFrameworkCore;

namespace MoneyFox.Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            EfCoreContext context = EfCoreContextFactory.Create();
            builder.RegisterInstance(context)
                   .As<DbContext>()
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}
