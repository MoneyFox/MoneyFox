using Android.Runtime;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Views.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.BackupFragment")]
    public class BackupFragment : BaseFragment<BackupViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_backup;
        protected override string Title => Strings.BackupLabel;

        public override async void OnStart()
        {
            base.OnStart();

            await ViewModel.LoadedCommand.ExecuteAsync();
        }
    }
}