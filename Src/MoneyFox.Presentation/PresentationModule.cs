using Autofac;
using MoneyFox.Presentation.ConverterBindings;
using MoneyFox.ServiceLayer;
using ReactiveUI;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ServiceModule>();

            builder.Register((c) => new StringToDoubleBinding()).As<IBindingTypeConverter>().SingleInstance();
        }
    }
}