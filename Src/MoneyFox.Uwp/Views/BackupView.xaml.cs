using MoneyFox.Application.Resources;

#nullable enable
namespace MoneyFox.Uwp.Views
{
    public sealed partial class BackupView
    {
        public override string Header => Strings.BackupTitle;

        public BackupView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.BackupVm;
        }
    }
}