using Autofac;
using MoneyFox.Core;
using MoneyFox.Win.Pages;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels;
using System;

namespace MoneyFox.Win
{
    internal class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();

            builder.RegisterType<NavigationService>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces()
                .AsSelf();

            NavigationService.Register<ShellViewModel, ShellPage>();
        }
    }
}
