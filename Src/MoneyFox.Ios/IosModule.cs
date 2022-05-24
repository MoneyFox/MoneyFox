namespace MoneyFox.iOS
{

    using System;
    using Autofac;

    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbPathProvider>().AsImplementedInterfaces();
            builder.RegisterType<GraphServiceClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<StoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<AppInformation>().AsImplementedInterfaces();
            builder.Register(c => new IosFileStore()).AsImplementedInterfaces(); // Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            builder.RegisterModule<MoneyFoxModule>();
        }
    }

}
