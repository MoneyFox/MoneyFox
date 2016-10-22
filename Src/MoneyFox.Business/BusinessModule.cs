using Autofac;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.Helpers;
using MoneyFox.Business.Manager;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Localization;
using MvvmCross.Plugins.ResxLocalization;

namespace MoneyFox.Business
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessModule>();

            builder.RegisterType<GlobalBusyIndicatorState>();
            builder.RegisterType<PasswordStorage>().As<IPasswordStorage>();
            builder.RegisterInstance(new MvxResxTextProvider(Strings.ResourceManager)).As<IMvxTextProvider>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("DataProvider"))
                .AsSelf()
                .SingleInstance();


            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .Where(x => x.Name != "SettingsManager")
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SettingsManager>()
                .UsingConstructor(typeof(ISettings))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .Where(x => !x.Name.StartsWith("DesignTime"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Session>();
        }
    }
}
