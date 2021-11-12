using AutoMapper;
using MoneyFox.ViewModels.Payments;

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
            var maps = MapperProfileHelper.LoadStandardMappings(typeof(PaymentViewModel).Assembly);

            foreach(var map in maps)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            var maps = MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);

            foreach(var map in maps)
            {
                map.CreateMappings(this);
            }
        }
    }
}