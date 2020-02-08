namespace MoneyFox.Uwp.Views
{
    public sealed partial class AddCategoryView
    {
        public override string Header => ViewModelLocator.AddCategoryVm.Title;

        public AddCategoryView()
        {
            InitializeComponent();
        }
    }
}
