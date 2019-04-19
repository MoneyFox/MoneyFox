using System;
using Autofac;
using MoneyFox.Presentation;

namespace MoneyFox.Windows
{
    public class WindowsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<PresentationModule>();
        }
    }
}
