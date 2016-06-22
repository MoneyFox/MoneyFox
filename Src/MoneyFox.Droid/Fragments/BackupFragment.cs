using Android.Runtime;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments {
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.BackupFragment")]
    public class BackupFragment : BaseFragment<BackupViewModel> {
        protected override int FragmentId => Resource.Layout.fragment_backup;
        protected override string Title => Strings.BackupLabel;

        public override void OnStart() {
            base.OnStart();

            ViewModel.LoadedCommand.Execute();
        }
    }
}