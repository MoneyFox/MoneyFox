namespace MoneyFox.Win.Mapping;

using AutoMapper;
using Core.Common.Interfaces.Mapping;
using System.Collections.Generic;
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
        IList<Map> mapsFromNotShared = MapperProfileHelper.LoadStandardMappings(typeof(PaymentViewModel).Assembly);

        foreach(Map map in mapsFromNotShared)
        {
            CreateMap(map.Source, map.Destination).ReverseMap();
        }

        IList<Map> mapsFromShared = MapperProfileHelper.LoadStandardMappings(typeof(AccountViewModel).Assembly);

        foreach(Map map in mapsFromShared)
        {
            CreateMap(map.Source, map.Destination).ReverseMap();
        }
    }

    private void LoadCustomMappings()
    {
        IList<IHaveCustomMapping> mapsFromNotShared =
            MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);

        foreach(IHaveCustomMapping map in mapsFromNotShared)
        {
            map.CreateMappings(this);
        }

        IList<IHaveCustomMapping> mapsFromShared =
            MapperProfileHelper.LoadCustomMappings(typeof(AccountViewModel).Assembly);

        foreach(IHaveCustomMapping map in mapsFromShared)
        {
            map.CreateMappings(this);
        }
    }
}