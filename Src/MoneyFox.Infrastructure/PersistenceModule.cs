using Autofac;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Infrastructure.OneDrive;
using MoneyFox.Infrastructure.Persistence;

namespace MoneyFox.Infrastructure
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => EfCoreContextFactory.Create())
                   .As<DbContext>()
                   .AsImplementedInterfaces();

            builder.RegisterType<ContextAdapter>().AsImplementedInterfaces();
            
            builder.RegisterType<OneDriveService>().AsImplementedInterfaces();
        }
    }
}
