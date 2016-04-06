using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using MvvmCross.Droid.Support.V7.Fragging.Presenter;

namespace MoneyManager.Droid
{
    public class CustomPresenter : MvxFragmentsPresenter
    {
        public CustomPresenter(IEnumerable<Assembly> AndroidViewAssemblies) : base(AndroidViewAssemblies)
        {
        }

        protected override void Show(Intent intent)
        {
            Activity.StartActivity(intent);
            Activity.OverridePendingTransition(Resource.Animation.abc_grow_fade_in_from_bottom, Resource.Animation.abc_fade_out);
        }

    }
}