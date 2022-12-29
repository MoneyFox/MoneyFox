namespace MoneyFox.Ui.Mapping;

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
