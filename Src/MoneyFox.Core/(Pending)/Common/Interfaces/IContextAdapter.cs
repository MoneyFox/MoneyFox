namespace MoneyFox.Core._Pending_.Common.Interfaces
{
    public interface IContextAdapter
    {
        IEfCoreContext Context { get; }

        void RecreateContext();
    }
}