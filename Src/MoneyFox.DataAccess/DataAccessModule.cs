using System;
using Autofac;
using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation;
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

                cfg.CreateMap<Payment, PaymentViewModel>()
                    .ForMember(dest => dest.Type,
                        opt => opt.MapFrom(src => (PaymentType) Enum.ToObject(typeof(PaymentType), src.Type)));
                cfg.CreateMap<PaymentViewModel, Payment>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int) src.Type));

                cfg.CreateMap<RecurringPayment, RecurringPaymentViewModel>()
                    .ForMember(dest => dest.Type,
                        opt => opt.MapFrom(src => (PaymentType) Enum.ToObject(typeof(PaymentType), src.Type)))
                    .ForMember(dest => dest.Recurrence,
                        opt => opt.MapFrom(src => (PaymentRecurrence) Enum.ToObject(typeof(PaymentRecurrence), src.Recurrence)));

                cfg.CreateMap<RecurringPaymentViewModel, RecurringPayment>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int) src.Type))
                    .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => (int) src.Recurrence));

                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.CreateMap<CategoryViewModel, Category>();
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
