using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using System;

namespace MoneyFox.DataAccess
{
    public static class MapperConfiguration
    {
        public static void Setup()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountViewModel>();
                cfg.CreateMap<AccountViewModel, Account>();

                cfg.CreateMap<Payment, PaymentViewModel>()
                    .ForMember(dest => dest.Type,
                        opt => opt.MapFrom(src => (PaymentType)Enum.ToObject(typeof(PaymentType), src.Type)));
                cfg.CreateMap<PaymentViewModel, Payment>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));

                cfg.CreateMap<RecurringPayment, RecurringPaymentViewModel>()
                    .ForMember(dest => dest.Type,
                        opt => opt.MapFrom(src => (PaymentType)Enum.ToObject(typeof(PaymentType), src.Type)))
                    .ForMember(dest => dest.Recurrence,
                        opt => opt.MapFrom(src => (PaymentRecurrence)Enum.ToObject(typeof(PaymentRecurrence), src.Recurrence)));

                cfg.CreateMap<RecurringPaymentViewModel, RecurringPayment>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type))
                    .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => (int)src.Recurrence));

                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.CreateMap<CategoryViewModel, Category>();
            });
        }
    }
}
