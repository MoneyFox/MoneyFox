using System;
using Autofac;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoneyFox.BusinessLogic;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessLogicModule>();

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

        private void SetupContextAndCrudServices(ContainerBuilder builder)
        {
            builder.RegisterInstance(SetupEfContext())
                   .As<DbContext>()
                   .AsSelf();

            SetUpCrudServices(builder);
        }

        private static EfCoreContext SetupEfContext()
        {
            var context = new EfCoreContext();
            context.Database.Migrate();

            return context;
        }

        private static void SetUpCrudServices(ContainerBuilder builder)
        {
            var context = SetupEfContext();

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
