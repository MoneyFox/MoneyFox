namespace MoneyFox
{
    using Autofac;
    using AutoMapper;
    using Core;
    using Mobile.Infrastructure;
    using System;
    using ViewModels.Settings;

    public class MoneyFoxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureMobileModule>();

            builder.RegisterInstance(AutoMapperFactory.Create());

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                .AsSelf();

            builder.RegisterAssemblyTypes(typeof(SettingsViewModel).Assembly)
                .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                .AsSelf();
        }
    }
}