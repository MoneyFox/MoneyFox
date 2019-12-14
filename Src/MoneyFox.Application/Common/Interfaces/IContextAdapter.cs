namespace MoneyFox.Application.Common.Interfaces
{
    public interface IContextAdapter
    {
        IEfCoreContext Context {get;}

        public void RecreateContext();
    }
}
