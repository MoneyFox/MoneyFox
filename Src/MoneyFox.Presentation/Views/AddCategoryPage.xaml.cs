namespace MoneyFox.Presentation.Views
{
	public partial class AddCategoryPage
    {
        public AddCategoryPage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AddCategoryVm;
        }
	}
}