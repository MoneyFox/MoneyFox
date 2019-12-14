namespace MoneyFox.Application.Common.Interfaces
{
    public interface IContextAdapter
    {
        IEfCoreContextAdapter Context {get;}

        public void RecreateContext();
    }
}
