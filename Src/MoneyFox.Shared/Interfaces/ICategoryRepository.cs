using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.Interfaces {
    public interface ICategoryRepository : IRepository<Category>, ISelectedProperty<Category> {
    }
}