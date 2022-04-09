namespace MoneyFox
{

    using System;
    using Autofac;
    using Core;
    using Mapping;
    using Mobile.Infrastructure;
    using ViewModels.Settings;

    public class MoneyFoxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureMobileModule>();
            builder.RegisterInstance(AutoMapperFactory.Create());
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith(value: "Service", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => !t.Name.StartsWith(value: "DesignTime", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith(value: "ViewModel", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsSelf();

            builder.RegisterAssemblyTypes(typeof(SettingsViewModel).Assembly)
                .Where(t => !t.Name.StartsWith(value: "DesignTime", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .Where(t => t.Name.EndsWith(value: "ViewModel", comparisonType: StringComparison.CurrentCultureIgnoreCase))
                .AsSelf();
        }
    }

}
