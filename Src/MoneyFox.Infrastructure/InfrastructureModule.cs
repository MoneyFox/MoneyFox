using Autofac;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Infrastructure.DbBackup;
using MoneyFox.Infrastructure.Persistence;

namespace MoneyFox.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<IContextAdapter>().Context)
                .AsImplementedInterfaces();

            builder.RegisterType<ContextAdapter>().AsImplementedInterfaces();
            builder.RegisterType<BackupService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveService>().AsImplementedInterfaces();
        }
    }
}