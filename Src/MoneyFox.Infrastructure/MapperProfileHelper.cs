using MoneyFox.Application.Common.Interfaces.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MoneyFox.Infrastructure
{
    public sealed class Map
    {
        public Type Source { get; set; }
        public Type Destination { get; set; }
    }

    public static class MapperProfileHelper
    {
        public static IList<Map> LoadStandardMappings(Assembly rootAssembly)
        {
            Type[] types = rootAssembly.GetExportedTypes();

            List<Map> mapsFrom = (
                from type in types
                from instance in type.GetInterfaces()
                where
                    instance.IsGenericType && instance.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                    !type.IsAbstract &&
                    !type.IsInterface
                select new Map
                {
                    Source = type.GetInterfaces().First(x => x.Name.Contains("MapFrom")).GetGenericArguments().First(),
                    Destination = type
                }).ToList();

            return mapsFrom;
        }

        public static IList<IHaveCustomMapping> LoadCustomMappings(Assembly rootAssembly)
        {
            Type[] types = rootAssembly.GetExportedTypes();

            List<IHaveCustomMapping> mapsFrom = (
                from type in types
                from instance in type.GetInterfaces()
                where
                    typeof(IHaveCustomMapping).IsAssignableFrom(type) &&
                    !type.IsAbstract &&
                    !type.IsInterface
                select (IHaveCustomMapping) Activator.CreateInstance(type)).ToList();

            return mapsFrom;
        }
    }
}
