using MoneyFox.Presentation;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class AddAccountView
    {
        public override string Header => ViewModelLocator.AddAccountVm.Title;

        public AddAccountView()
        {
            InitializeComponent();
        }
    }
}
