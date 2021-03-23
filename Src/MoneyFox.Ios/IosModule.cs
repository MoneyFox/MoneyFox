using Acr.UserDialogs;
using Autofac;
using Foundation;
using GalaSoft.MvvmLight.Messaging;
using MoneyFox.Application;
using MoneyFox.iOS.Src;
using NLog;
using System;
using System.Globalization;

namespace MoneyFox.iOS
{
    public class IosModule : Module
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GraphServiceClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<StoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<AppInformation>().AsImplementedInterfaces();
            builder.Register(c => UserDialogs.Instance).As<IUserDialogs>();
            builder.Register(c => new IosFileStore(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))).AsImplementedInterfaces();
            builder.RegisterInstance(Messenger.Default).AsImplementedInterfaces();

            SetLocale();
            builder.RegisterModule<MoneyFoxModule>();
        }

        private void SetLocale()
        {
            try
            {
                logManager.Info($"Current country code: {NSLocale.CurrentLocale.CountryCode}.");
                CultureHelper.CurrentLocale = new CultureInfo(NSLocale.CurrentLocale.CountryCode);
            }
            catch(Exception ex)
            {
                logManager.Error(ex);
                CultureHelper.CurrentLocale = CultureInfo.CurrentCulture;
            }
        }
    }
}
