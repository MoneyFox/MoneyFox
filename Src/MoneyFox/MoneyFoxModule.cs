﻿using Autofac;
using MoneyFox.Application;
using MoneyFox.AutoMapper;
using MoneyFox.Infrastructure;
using MoneyFox.Mobile.Infrastructure;
using MoneyFox.ViewModels.Settings;
using System;

namespace MoneyFox
{
    public class MoneyFoxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ApplicationModule>();

            builder.RegisterModule<InfrastructureMobile>();
            builder.RegisterModule<InfrastructureModule>();

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