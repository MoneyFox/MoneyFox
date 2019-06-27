using Microsoft.EntityFrameworkCore;

namespace MoneyFox.DataLayer
{
    public class EfCoreContextFactory
    {
        public EfCoreContext Build()
        {
            EfCoreContext context = new EfCoreContext();
            context.Database.Migrate();
            return context;
        }
    }
}
