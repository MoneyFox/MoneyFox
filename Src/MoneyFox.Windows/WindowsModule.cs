using Autofac;
using MoneyFox.Presentation;
using MoneyFox.Windows.ConverterBindings;
using MvvmCross.Plugin.File.Platforms.Uap;
using ReactiveUI;

namespace MoneyFox.Windows
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<PresentationModule>();

            builder.RegisterType<DialogService>().AsImplementedInterfaces();
            builder.RegisterType<ProtectedData>().AsImplementedInterfaces();
            builder.RegisterType<WindowsAppInformation>().AsImplementedInterfaces();
            builder.RegisterType<MarketplaceOperations>().AsImplementedInterfaces();
            builder.RegisterType<BackgroundTaskManager>().AsImplementedInterfaces();
            builder.RegisterType<MvxWindowsFileStore>().AsImplementedInterfaces();

            //builder.Register((c) => new ObservableCollectionToItemCollectionBinding()).As<IBindingTypeConverter>().SingleInstance();
            builder.Register((c) => new ListToItemCollectionBinding()).As<IBindingTypeConverter>().SingleInstance();
        }
    }
}
