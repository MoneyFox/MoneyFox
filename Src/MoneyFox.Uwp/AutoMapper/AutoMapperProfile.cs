using AutoMapper;
using MoneyFox.Uwp.ViewModels.Accounts;
using MoneyFox.Uwp.ViewModels.Payments;

#nullable enable
namespace MoneyFox.Uwp.AutoMapper
{
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

            foreach(var map in mapsFromNotShared)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }

            var mapsFromShared = MapperProfileHelper.LoadStandardMappings(typeof(AccountViewModel).Assembly);

            foreach(var map in mapsFromShared)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            var mapsFromNotShared =
                MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);

            foreach(var map in mapsFromNotShared)
            {
                map.CreateMappings(this);
            }

            var mapsFromShared =
                MapperProfileHelper.LoadCustomMappings(typeof(AccountViewModel).Assembly);

            foreach(var map in mapsFromShared)
            {
                map.CreateMappings(this);
            }
        }
    }
}