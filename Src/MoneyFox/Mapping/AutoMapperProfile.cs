namespace MoneyFox.Mapping
{

    using AutoMapper;
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
            var maps = MapperProfileHelper.LoadStandardMappings(typeof(PaymentViewModel).Assembly);
            foreach (var map in maps)
            {
                CreateMap(sourceType: map.Source, destinationType: map.Destination).ReverseMap();
            }
        }

        private void LoadCustomMappings()
        {
            var maps = MapperProfileHelper.LoadCustomMappings(typeof(PaymentViewModel).Assembly);
            foreach (var map in maps)
            {
                map.CreateMappings(this);
            }
        }
    }

}
