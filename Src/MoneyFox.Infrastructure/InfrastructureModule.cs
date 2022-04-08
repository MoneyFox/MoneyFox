namespace MoneyFox.Infrastructure
{

    using Autofac;
    using Core.Common.Interfaces;
    using DbBackup;
    using Persistence;

    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<IContextAdapter>().Context).AsImplementedInterfaces();
            builder.RegisterType<ContextAdapter>().AsImplementedInterfaces();
            builder.RegisterType<BackupService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveAuthenticationService>().AsImplementedInterfaces();
        }
    }

}
