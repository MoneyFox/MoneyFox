using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Shared.Attributes;

namespace MoneyFox.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.CategoriesFragment")]
    public class CategoriesFragment : BaseFragment<CategoryListViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_category_list;
        protected override string Title => Strings.CategoriesLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var categoryList = view.FindViewById<MvxListView>(Resource.Id.category_list);
            RegisterForContextMenu(categoryList);

            view.FindViewById<FloatingActionButton>(Resource.Id.fab_create_category).Click += (s, e) =>
            {
                ViewModel.CreateNewCategoryCommand.Execute();
            };

            return view;
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.category_list)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(0, 0, 0, Strings.EditLabel);
                menu.Add(0, 1, 1, Strings.DeleteLabel);
            }
        }

        public override void OnStart()
        {
            ViewModel.LoadedCommand.Execute();
            base.OnStart();
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = ViewModel.Categories[((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position];

            switch (item.ItemId)
            {
                case 0:
                    ViewModel.EditCategoryCommand.Execute(selected);
                    return true;

                case 1:
                    ViewModel.DeleteCategoryCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }
    }
}