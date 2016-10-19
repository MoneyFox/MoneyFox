using Autofac;

namespace MoneyFox.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<Contact, ContactViewModel>();
            //    cfg.CreateMap<ContactViewModel, Contact>();
            //});

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
