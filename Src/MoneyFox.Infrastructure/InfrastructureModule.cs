namespace MoneyFox.Infrastructure
{

    using Autofac;
    using Core.Common.Mediatr;
    using DataAccess;
    using DbBackup;
    using DbBackup.Legacy;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.Interfaces;
    using Persistence;

    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => EfCoreContextFactory.Create(
                c.Resolve<ICustomPublisher>(),
                c.Resolve<ISettingsFacade>(),
                c.Resolve<IDbPathProvider>().GetDbPath())).AsSelf().AsImplementedInterfaces();
            builder.RegisterType<ContextAdapter>().AsImplementedInterfaces();

            RegisterOneDriveServices(builder);
            RegisterRepositories(builder);
        }

        private static void RegisterOneDriveServices(ContainerBuilder builder)
        {
            builder.RegisterType<OneDriveAuthenticationService>().AsImplementedInterfaces();
            builder.RegisterType<OneDriveBackupUploadService>().AsImplementedInterfaces();

            builder.RegisterType<OneDriveService>().AsImplementedInterfaces();
            builder.RegisterType<BackupService>().AsImplementedInterfaces();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryRepository>().AsImplementedInterfaces();
        }
    }

}
