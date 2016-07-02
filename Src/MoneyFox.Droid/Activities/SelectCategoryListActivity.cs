using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MoneyFox.Droid.Dialogs;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MoneyFox.Droid.Activities {
    [Activity(Label = "CategoryListActivity",
        Name = "moneyfox.droid.activities.SelectCategoryListActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class SelectCategoryListActivity : MvxFragmentCompatActivity<SelectCategoryListViewModel>,
        IDialogCloseListener {
        public void HandleDialogClose() {
            // Make an empty search to refresh the list and groups
            var selectCategoryListViewModel = ViewModel;
            if (selectCategoryListViewModel != null) {
                selectCategoryListViewModel.SearchText = string.Empty;
                selectCategoryListViewModel.Search();
            }
        }

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_select_category_list);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var list = FindViewById<ListView>(Resource.Id.category_list);
            RegisterForContextMenu(list);

            FindViewById<FloatingActionButton>(Resource.Id.fab_create_category).Click += (s, e) => {
                var dialog = new ModifyCategoryDialog {
                    ViewModel = Mvx.Resolve<ModifyCategoryDialogViewModel>()
                };

                dialog.Show(FragmentManager, Strings.AddCategoryTitle);
            };

            Title = Strings.ChooseCategorieTitle;
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu) {
            MenuInflater.Inflate(Resource.Menu.menu_select, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        ///     This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        public override bool OnOptionsItemSelected(IMenuItem item) {
            switch (item.ItemId) {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.action_add:
                    var dialog = new ModifyCategoryDialog {
                        ViewModel = Mvx.Resolve<ModifyCategoryDialogViewModel>()
                    };

                    dialog.Show(FragmentManager, "dialog");
                    return true;

                default:
                    return false;
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo) {
            if (v.Id == Resource.Id.category_list) {
                menu.SetHeaderTitle(Strings.SelectOperationLabel);
                menu.Add(Strings.EditLabel);
                menu.Add(Strings.DeleteLabel);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item) {
            var selected = ViewModel.Categories[((AdapterView.AdapterContextMenuInfo) item.MenuInfo).Position];

            switch (item.ItemId) {
                case 0:
                    OpenEditCategoryDialog();
                    return true;

                case 1:
                    ViewModel.DeleteCategoryCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }

        private void OpenEditCategoryDialog() {
            var viewmodel = Mvx.Resolve<ModifyCategoryDialogViewModel>();
            viewmodel.IsEdit = true;
            var dialog = new ModifyCategoryDialog {
                ViewModel = viewmodel
            };

            dialog.Show(FragmentManager, "dialog");
        }
    }
}