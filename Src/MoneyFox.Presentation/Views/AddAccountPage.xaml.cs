namespace MoneyFox.Presentation.Views
{
	public partial class AddAccountPage
	{
		public AddAccountPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddAccountVm;
        }
	}
}