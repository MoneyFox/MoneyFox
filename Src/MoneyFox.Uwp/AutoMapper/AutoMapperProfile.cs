using AutoMapper;
using MoneyFox.Application.Common.Interfaces.Mapping;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Backup;
using System.Collections.Generic;

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
            IList<Map> mapsFromNotShared = MapperProfileHelper.LoadStandardMappings(typeof(UserAccountViewModel).Assembly);

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
            IList<IHaveCustomMapping> mapsFromNotShared = MapperProfileHelper.LoadCustomMappings(typeof(UserAccountViewModel).Assembly);

            foreach(IHaveCustomMapping map in mapsFromNotShared)
            {
                map.CreateMappings(this);
            }

            IList<IHaveCustomMapping> mapsFromShared = MapperProfileHelper.LoadCustomMappings(typeof(AccountViewModel).Assembly);

            foreach(IHaveCustomMapping map in mapsFromShared)
            {
                map.CreateMappings(this);
            }
        }
    }
}
