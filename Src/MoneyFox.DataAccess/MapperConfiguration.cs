using AutoMapper;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using System;
using MoneyFox.DataAccess.Entities;

namespace MoneyFox.DataAccess
{
    public static class MapperConfiguration
    {
        public static void Setup()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AccountEntity, AccountViewModel>();
                cfg.CreateMap<AccountViewModel, AccountEntity>();

                cfg.CreateMap<PaymentEntity, PaymentViewModel>()
                    .ForMember(dest => dest.Type,
                        opt => opt.MapFrom(src => (PaymentType)Enum.ToObject(typeof(PaymentType), src.Type)));
                cfg.CreateMap<PaymentViewModel, PaymentEntity>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type));

                cfg.CreateMap<RecurringPaymentEntity, RecurringPaymentViewModel>()
                    .ForMember(dest => dest.Type,
                        opt => opt.MapFrom(src => (PaymentType)Enum.ToObject(typeof(PaymentType), src.Type)))
                    .ForMember(dest => dest.Recurrence,
                        opt => opt.MapFrom(src => (PaymentRecurrence)Enum.ToObject(typeof(PaymentRecurrence), src.Recurrence)));

                cfg.CreateMap<RecurringPaymentViewModel, RecurringPaymentEntity>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type))
                    .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => (int)src.Recurrence));

                cfg.CreateMap<CategoryEntity, CategoryViewModel>();
                cfg.CreateMap<CategoryViewModel, CategoryEntity>();
            });
        }
    }
}
