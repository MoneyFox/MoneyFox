namespace MoneyFox.Infrastructure
{

    using Autofac;
    using Core.Common.Interfaces;
    using DataAccess;
    using DbBackup;
    using Persistence;

    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ContextAdapter>().AsImplementedInterfaces();
            builder.Register(c => c.Resolve<IContextAdapter>().Context).AsImplementedInterfaces();

            RegisterOneDriveServices(builder);
            RegisterRepositories(builder);
        }

        private static void RegisterOneDriveServices(ContainerBuilder builder)
        {
            builder.RegisterType<BackupService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveAuthenticationService>().AsImplementedInterfaces();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryRepository>().AsImplementedInterfaces();
        }
    }

}
