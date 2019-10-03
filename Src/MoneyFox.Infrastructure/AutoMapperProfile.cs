using System.Reflection;
using AutoMapper;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Infrastructure
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
            var mapsFrom = MapperProfileHelper.LoadStandardMappings(typeof(PaymentViewModel).Assembly);

            foreach (var map in mapsFrom)
            {
                CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            var mapsFrom = MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);

            foreach (var map in mapsFrom)
            {
                map.CreateMappings(this);
            }
        }
    }
}
