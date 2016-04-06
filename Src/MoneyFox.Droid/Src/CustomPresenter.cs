using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Content;
using MoneyManager.Droid;
using MvvmCross.Droid.Support.V7.Fragging.Presenter;

namespace MoneyFox.Droid
{
    public class CustomPresenter : MvxFragmentsPresenter
    {
        public CustomPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        protected override void Show(Intent intent)
        {
            Activity.StartActivity(intent);
            Activity.OverridePendingTransition(Resource.Animation.abc_grow_fade_in_from_bottom, Resource.Animation.abc_fade_out);
        }

    }
}