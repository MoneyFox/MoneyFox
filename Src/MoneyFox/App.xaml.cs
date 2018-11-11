namespace MoneyFox
{
	public partial class App
    {
		public App ()
		{
		    StyleHelper.Init();
            InitializeComponent();
        }

        protected override void OnStart()
        {
            //if (Mvx.IoCProvider.Resolve<ISettingsManager>().Theme == AppTheme.Dark)
            //{
            //    Application.Current.Resources.Add(new ColorsDark());
            //} else
            //{
            //    Application.Current.Resources.Add(new ColorsLight());
            //}
        }
    }
}
