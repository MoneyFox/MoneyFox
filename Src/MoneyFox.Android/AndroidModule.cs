namespace MoneyFox.Droid
{
    using Acr.UserDialogs;
    using Android.App;
    using Autofac;
    using Src;

    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbPathProvider>().AsImplementedInterfaces();
            builder.RegisterType<GraphClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<PlayStoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<DroidAppInformation>().AsImplementedInterfaces();
            builder.Register(c => UserDialogs.Instance).As<IUserDialogs>();
            builder.Register(c => new FileStoreIoBase(Application.Context.FilesDir?.Path ?? ""))
                .AsImplementedInterfaces();

            builder.RegisterModule<MoneyFoxModule>();
        }
    }
}