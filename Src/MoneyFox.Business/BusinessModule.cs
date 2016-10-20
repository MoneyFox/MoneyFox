using Autofac;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.Helpers;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Plugins.ResxLocalization;

namespace MoneyFox.Business
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessModule>();

            Mvx.RegisterSingleton(() => new GlobalBusyIndicatorState());
            Mvx.RegisterSingleton<IPasswordStorage>(new PasswordStorage(Mvx.Resolve<IProtectedData>()));
            Mvx.RegisterSingleton<IMvxTextProvider>(new MvxResxTextProvider(Strings.ResourceManager));


            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .SingleInstance();

        }
    }
}
