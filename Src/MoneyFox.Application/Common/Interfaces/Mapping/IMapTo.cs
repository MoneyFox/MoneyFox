using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Application.Common.Interfaces.Mapping
{
    [SuppressMessage("Minor Code Smell", "S4023:Interfaces should not be empty", Justification = "Marker Interface")]
    public interface IMapTo<TEntity>
    {
    }
}
