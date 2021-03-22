using Acr.UserDialogs;
using Autofac;
using Foundation;
using GalaSoft.MvvmLight.Messaging;
using MoneyFox.Application;
using MoneyFox.iOS.Src;
using System;
using System.Globalization;

namespace MoneyFox.iOS
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GraphServiceClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<StoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<AppInformation>().AsImplementedInterfaces();
            builder.Register(c => UserDialogs.Instance).As<IUserDialogs>();
            builder.Register(c => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))).AsImplementedInterfaces();
            builder.RegisterInstance(Messenger.Default).AsImplementedInterfaces();

            CultureHelper.CurrentLocale = new CultureInfo(NSLocale.CurrentLocale.CountryCode);

            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}
