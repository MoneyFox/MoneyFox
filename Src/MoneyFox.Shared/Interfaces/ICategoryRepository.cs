using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;

namespace MoneyFox.Shared.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>, IData<Category>
    {
        
    }
}