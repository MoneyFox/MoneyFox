using Android.Runtime;
using MoneyManager.Core.ViewModels;
using MoneyManager.Droid.Fragments;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.BackupFragment")]
    public class BackupFragment : BaseFragment<BackupViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_backup;
    }
}