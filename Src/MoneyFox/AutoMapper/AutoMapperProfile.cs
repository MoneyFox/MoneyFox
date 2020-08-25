using AutoMapper;
using MoneyFox.Application.Common.Interfaces.Mapping;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using System.Collections.Generic;

namespace MoneyFox.AutoMapper
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
            IList<Map> maps = MapperProfileHelper.LoadStandardMappings(typeof(PaymentViewModel).Assembly);

            foreach(Map map in maps)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            IList<IHaveCustomMapping> maps = MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);

            foreach(IHaveCustomMapping map in maps)
            {
                map.CreateMappings(this);
            }
        }
    }
}
