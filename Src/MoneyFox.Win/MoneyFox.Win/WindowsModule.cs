using Autofac;
using MoneyFox.Core;
using MoneyFox.Win.Pages;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels;

namespace MoneyFox.Win
{
    internal class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();

            builder.RegisterType<NavigationService>().AsImplementedInterfaces().SingleInstance();

            NavigationService.Register<ShellViewModel, ShellPage>();
        }
    }
}
