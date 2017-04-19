using System.Threading.Tasks;

namespace MoneyFox.DataAccess
{
    /// <summary>
    ///     Enables to save changes made over several repositories in one transaction.
    /// </summary>
    public interface IUnitOfWork
    {
        Task Commit();
    }
}