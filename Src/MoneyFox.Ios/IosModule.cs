using Autofac;
using GalaSoft.MvvmLight.Messaging;
using MoneyFox.iOS.Src;
using System;

namespace MoneyFox.iOS
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GraphServiceClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<StoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<AppInformation>().AsImplementedInterfaces();
            builder.Register(c => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))).AsImplementedInterfaces();
            builder.RegisterInstance(Messenger.Default).AsImplementedInterfaces();

            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}