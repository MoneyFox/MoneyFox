namespace MoneyFox.Droid
{
    using Autofac;
    using global::Android.Content;

    public class AndroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbPathProvider>().AsImplementedInterfaces();
            builder.RegisterType<GraphClientFactory>().AsImplementedInterfaces();
            builder.RegisterType<PlayStoreOperations>().AsImplementedInterfaces();
            builder.RegisterType<DroidAppInformation>().AsImplementedInterfaces();
            builder.Register(c => new FileStoreIoBase(Context.FilesDir?.Path ?? "")).AsImplementedInterfaces();
            builder.RegisterModule<MoneyFoxModule>();
        }
    }

}
