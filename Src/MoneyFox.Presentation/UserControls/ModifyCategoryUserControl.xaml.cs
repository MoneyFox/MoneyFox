using System.Reactive.Disposables;
using ReactiveUI;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyCategoryUserControl
	{
		public ModifyCategoryUserControl ()
		{
			InitializeComponent ();

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SelectedCategory.Name, v => v.CategoryNameLabel).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedCategory.Note, v => v.NoteLabel).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["CategoryNameLabel"],
                                v => v.CategoryNameLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["NoteLabel"],
                                v => v.CategoryNameLabel.Text).DisposeWith(disposables);
            });
        }
	}
}