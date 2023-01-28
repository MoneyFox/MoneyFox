namespace MoneyFox.Ui.Mapping;

using AutoMapper;
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
