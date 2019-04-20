using System;
using Autofac;
using GenericServices;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using Microsoft.EntityFrameworkCore;
using MoneyFox.BusinessLogic;
using MoneyFox.DataLayer;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.ServiceLayer
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessLogicModule>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.InvariantCulture))
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Facade", StringComparison.InvariantCulture))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            SetupContextAndCrudServices(builder);
        }

        private void SetupContextAndCrudServices(ContainerBuilder builder) {
            var context = SetupEfContext();

            builder.Register((c) => context);
            builder.Register((c) => SetUpCrudServices(context));
        }

        private static EfCoreContext SetupEfContext() {
            var context = new EfCoreContext();
            context.Database.Migrate();

            return context;
        }

        private static ICrudServicesAsync SetUpCrudServices(EfCoreContext context) {
            var utData = context.SetupSingleDtoAndEntities<AccountViewModel>();
            utData.AddSingleDto<CategoryViewModel>();
            utData.AddSingleDto<PaymentViewModel>();
            utData.AddSingleDto<RecurringPaymentViewModel>();

            return new CrudServicesAsync(context, utData.ConfigAndMapper);
        }
    }
}