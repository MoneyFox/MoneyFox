namespace MoneyFox.Core.Common.Interfaces
{
    public interface IContextAdapter
    {
        IAppDbContext Context { get; }

        void RecreateContext();
    }
}