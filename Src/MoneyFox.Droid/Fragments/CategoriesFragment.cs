using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof (MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.CategoriesFragment")]
    public class CategoriesFragment : BaseFragment<CategoryListViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_category_list;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var list = view.FindViewById<ListView>(Resource.Id.category_list);
            RegisterForContextMenu(list);
            HasOptionsMenu = true;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_add, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.category_list)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(Strings.EditLabel);
                menu.Add(Strings.DeleteLabel);
            }
        }

        /// <summary>
        ///     This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    var dialog = new ModifyCategoryDialog
                    {
                        ViewModel = Mvx.Resolve<CategoryDialogViewModel>()
                    };

                    dialog.Show(Activity.FragmentManager, "dialog");
                    return true;

                default:
                    return false;
            }
        }
    }
}