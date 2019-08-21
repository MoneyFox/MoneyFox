using System;
using Autofac;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoneyFox.Application;
using MoneyFox.Application.Constants;
using MoneyFox.BusinessLogic;
using MoneyFox.Persistence;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessLogicModule>();
            builder.RegisterModule<ApplicationModule>();

            SetupContextAndCrudServices(builder);

            builder.RegisterType<CrudServicesAsync>().AsImplementedInterfaces();

            builder.Register(c => PublicClientApplicationBuilder
                                   .Create(ServiceConstants.MSAL_APPLICATION_ID)
                                   .WithRedirectUri($"msal{ServiceConstants.MSAL_APPLICATION_ID}://auth")
                                   .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                                   .Build());

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => !t.Name.Equals("NavigationService", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.Name.EndsWith("Facade", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => !t.Name.StartsWith("DesignTime", StringComparison.CurrentCultureIgnoreCase))
                   .Where(t => t.Name.EndsWith("ViewModel", StringComparison.CurrentCultureIgnoreCase))
                   .AsSelf();
        }

        private static void SetupContextAndCrudServices(ContainerBuilder builder)
        {
            var context = EfCoreContextFactory.Create();

            builder.RegisterInstance(context)
                   .As<DbContext>()
                   .AsImplementedInterfaces()
                   .AsSelf();

            SetUpCrudServices(builder, context);
        }

        private static void SetUpCrudServices(ContainerBuilder builder, EfCoreContext context)
        {
            var utData = context.SetupSingleDtoAndEntities<AccountViewModel>();
            utData.AddSingleDto<CategoryViewModel>();
            utData.AddSingleDto<PaymentViewModel>();
            utData.AddSingleDto<RecurringPaymentViewModel>();

            var crudService = new CrudServices(context, utData.ConfigAndMapper);
            var crudServiceAsync = new CrudServicesAsync(context, utData.ConfigAndMapper);

            builder.Register(c => crudService);
            builder.Register(c => crudServiceAsync);
            builder.Register(c => utData.ConfigAndMapper);
        }
    }
}
