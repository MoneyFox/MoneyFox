using Autofac;
using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountViewModel>();
                cfg.CreateMap<AccountViewModel, Account>();

                cfg.CreateMap<Payment, PaymentViewModel>();
                cfg.CreateMap<PaymentViewModel, Payment>();

                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.CreateMap<CategoryViewModel, Category>();

                cfg.CreateMap<RecurringPayment, RecurringPaymentViewModel>();
                cfg.CreateMap<RecurringPaymentViewModel, RecurringPayment>();
            });

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
