using AutoMapper;

namespace MoneyFox.Application.Common.Interfaces.Mapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}
