namespace MoneyFox.Core.Common.Interfaces.Mapping;

using AutoMapper;

public interface IHaveCustomMapping
{
    void CreateMappings(Profile configuration);
}
