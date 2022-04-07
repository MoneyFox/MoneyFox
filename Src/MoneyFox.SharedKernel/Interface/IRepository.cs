namespace MoneyFox.SharedKernel.Interface
{

    public interface IRepository<T> where T : class, IAggregateRoot { }

}
