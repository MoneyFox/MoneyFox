namespace MoneyFox.Win.Common.Mapping;

using AutoMapper;
using ViewModels.Accounts;
using ViewModels.Payments;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        LoadStandardMappings();
        LoadCustomMappings();
    }

    private void LoadStandardMappings()
    {
        var mapsFromNotShared = MapperProfileHelper.LoadStandardMappings(typeof(PaymentViewModel).Assembly);
        foreach (var map in mapsFromNotShared)
        {
            CreateMap(sourceType: map.Source, destinationType: map.Destination).ReverseMap();
        }

        var mapsFromShared = MapperProfileHelper.LoadStandardMappings(typeof(AccountViewModel).Assembly);
        foreach (var map in mapsFromShared)
        {
            CreateMap(sourceType: map.Source, destinationType: map.Destination).ReverseMap();
        }
    }

    private void LoadCustomMappings()
    {
        var mapsFromNotShared = MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);
        foreach (var map in mapsFromNotShared)
        {
            map.CreateMappings(this);
        }

        var mapsFromShared = MapperProfileHelper.LoadCustomMappings(typeof(AccountViewModel).Assembly);
        foreach (var map in mapsFromShared)
        {
            map.CreateMappings(this);
        }
    }
}
