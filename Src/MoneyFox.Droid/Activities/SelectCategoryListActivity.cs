using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "CategoryListActivity",
        Name = "moneyfox.droid.activities.SelectCategoryListActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class SelectCategoryListActivity : MvxAppCompatActivity<SelectCategoryListViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_select_category_list);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var list = FindViewById<MvxRecyclerView>(Resource.Id.category_list);
            RegisterForContextMenu(list);

            FindViewById<FloatingActionButton>(Resource.Id.fab_create_category).Click += (s, e) =>
            {
                ViewModel.CreateNewCategoryCommand.Execute();
            };

            Title = Strings.ChooseCategorieTitle;
        }

        /// <summary>
        ///     Refresh list of categories
        /// </summary>
        public async void RefreshCategories()
        {
            // Make an empty search to refresh the list and groups
            var selectCategoryListViewModel = ViewModel;
            if (selectCategoryListViewModel != null)
            {
                await selectCategoryListViewModel.Search();
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
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.action_add:
                    ViewModel.CreateNewCategoryCommand.Execute();
                    RefreshCategories();
                    return true;

                default:
                    return false;
            }
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

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = ViewModel.Categories[((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position];

            switch (item.ItemId)
            {
                case 0:
                    ViewModel.EditCategoryCommand.Execute(selected);
                    RefreshCategories();
                    return true;

                case 1:
                    ViewModel.DeleteCategoryCommand.Execute(selected);
                    RefreshCategories();
                    return true;

                default:
                    return false;
            }
        }
    }
}