namespace MoneyFox.iOS
{
    using Acr.UserDialogs;
    using Autofac;
    using NLog;
    using Src;
    using System;

    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbPathProvider>().AsImplementedInterfaces();
            builder.RegisterType<GraphServiceClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<StoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<AppInformation>().AsImplementedInterfaces();
            builder.Register(c => UserDialogs.Instance).As<IUserDialogs>();
            builder.Register(c
                    => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
                .AsImplementedInterfaces();

            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}
