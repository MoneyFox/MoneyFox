﻿using AutoMapper;

#nullable enable
namespace MoneyFox.Uwp.AutoMapper
{
    public static class AutoMapperFactory
    {
        public static IMapper Create()
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new AutoMapperProfile());
                });

            return mappingConfig.CreateMapper();
        }
    }
}