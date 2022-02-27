namespace MoneyFox.AutoMapper
{
    using Core._Pending_.Common.Interfaces.Mapping;
    using global::AutoMapper;
    using System.Collections.Generic;
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