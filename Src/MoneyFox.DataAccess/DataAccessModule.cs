using Autofac;
using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation.DataModels;

namespace MoneyFox.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountViewModel>();
                cfg.CreateMap<AccountViewModel, Account>();
            });

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
