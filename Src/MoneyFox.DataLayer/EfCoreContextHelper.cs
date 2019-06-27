using Microsoft.EntityFrameworkCore;

namespace MoneyFox.DataLayer
{
    public class EfCoreContextHelper
    {
        public EfCoreContext CreateContext()
        {
            EfCoreContext context = new EfCoreContext();
            context.Database.Migrate();
            return context;
        }
    }
}
