using Autofac;
using MoneyFox.ServiceLayer;
using MoneyFox.Windows.Converter;
using ReactiveUI;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceModule>();

            builder.Register((c) => new StringToDoubleBindingTypeConverter()).As<IBindingTypeConverter>()
                   .SingleInstance();
        }
    }
}