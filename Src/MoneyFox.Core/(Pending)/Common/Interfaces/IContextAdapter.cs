namespace MoneyFox.Core._Pending_.Common.Interfaces
{
    public interface IContextAdapter
    {
        IAppDbContext Context { get; }

        void RecreateContext();
    }
}