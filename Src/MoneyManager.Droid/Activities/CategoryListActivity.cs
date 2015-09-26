using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using MoneyManager.Core.ViewModels;
using MoneyManager.Localization;

namespace MoneyManager.Droid.Activities
{
    [Activity(Label = "CategoryListActivity")]
    public class CategoryListActivity : MvxActivity
    {
        public new CategoryListViewModel ViewModel
        {
            get { return (CategoryListViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CategoryListLayout);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.SelectMenu, menu);
            return base.OnCreateOptionsMenu(menu);
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

                case Resource.Id.action_done:
                    ViewModel.DoneCommand.Execute(null);
                    return true;

                case Resource.Id.action_add:

                    return true;

                default:
                    return false;
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.accountList)
            {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(Strings.EditLabel);
                menu.Add(Strings.DeleteLabel);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var selected = ViewModel.Categories[((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position];

            switch (item.ItemId)
            {
                case 0:
                    //TODO: Implement edit
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