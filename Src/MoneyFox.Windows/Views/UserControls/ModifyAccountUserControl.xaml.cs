using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyModifyAccountUserControl : ReactiveUserControl<ModifyAccountViewModel>
    {
    }

    public sealed partial class ModifyAccountUserControl
    {
        public ModifyAccountUserControl()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SelectedAccount.Name, v => NameTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedAccount.CurrentBalance, v => CurrentBalanceTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedAccount.Note, v => NoteTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedAccount.IsExcluded, v => IsExcludedCheckBox.IsChecked).DisposeWith(disposables);

                CurrentBalanceTextBox.Events()
                                     .GotFocus
                                     .Do(args => CurrentBalanceTextBox.SelectAll());
            });
        }
    }
}