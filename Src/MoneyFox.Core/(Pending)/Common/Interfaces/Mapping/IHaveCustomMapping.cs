namespace MoneyFox.Core._Pending_.Common.Interfaces.Mapping
{
    using AutoMapper;

    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}