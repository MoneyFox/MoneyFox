namespace MoneyFox.Mapping;

using System.Reflection;
using Core.Common.Interfaces.Mapping;

public sealed class Map
{
    public Map(Type source, Type destination)
    {
        Source = source;
        Destination = destination;
    }

    public Type Source { get; }

    public Type Destination { get; }
}

public static class MapperProfileHelper
{
    public static IList<Map> LoadStandardMappings(Assembly rootAssembly)
    {
        var types = rootAssembly.GetExportedTypes();
        var mapsFrom = (from type in types
            from instance in type.GetInterfaces()
            where instance.IsGenericType && instance.GetGenericTypeDefinition() == typeof(IMapFrom<>) && !type.IsAbstract && !type.IsInterface
            select new Map(source: type.GetInterfaces().First(x => x.Name.Contains("MapFrom")).GetGenericArguments().First(), destination: type)).ToList();

        return mapsFrom;
    }

    public static IList<IHaveCustomMapping> LoadCustomMappings(Assembly rootAssembly)
    {
        var types = rootAssembly.GetExportedTypes();
        var mapsFrom = (from type in types
            from instance in type.GetInterfaces()
            where typeof(IHaveCustomMapping).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
            select (IHaveCustomMapping)Activator.CreateInstance(type)).ToList();

        return mapsFrom;
    }
}
