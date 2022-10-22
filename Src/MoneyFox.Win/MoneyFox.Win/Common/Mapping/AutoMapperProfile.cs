namespace MoneyFox.Win.Common.Mapping;

using AutoMapper;
using ViewModels.Payments;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        var mappings = MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);
        foreach (var map in mappings)
        {
            map.CreateMappings(this);
        }
    }
}
