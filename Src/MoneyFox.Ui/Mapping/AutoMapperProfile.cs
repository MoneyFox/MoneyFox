namespace MoneyFox.Ui.Mapping;

using AutoMapper;
using MoneyFox.Ui.Views.Payments.PaymentList;
using MoneyFox.Ui.Views.Payments.PaymentModification;
using Views.Payments;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        var maps = MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);
        foreach (var map in maps)
        {
            map.CreateMappings(this);
        }
    }
}
