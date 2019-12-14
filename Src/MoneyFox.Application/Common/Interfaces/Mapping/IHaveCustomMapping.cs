using AutoMapper;

namespace MoneyFox.Application.Interfaces.Mapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}
