using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
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
        protected override string Title => Strings.CategoriesLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var list = view.FindViewById<ListView>(Resource.Id.category_list);
            RegisterForContextMenu(list);

            var button = view.FindViewById<FloatingActionButton>(Resource.Id.fab_create_category);
            button.Click += (s, e) =>
            {
                var dialog = new ModifyCategoryDialog
                {
                    ViewModel = Mvx.Resolve<CategoryDialogViewModel>()
                };

                dialog.Show(Activity.FragmentManager, Strings.AddCategoryTitle);
            };

            return view;
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
    }
}