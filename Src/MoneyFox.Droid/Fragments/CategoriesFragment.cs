using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MoneyFox.Droid.Dialogs;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Platform;

namespace MoneyFox.Droid.Fragments {
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("moneyfox.droid.fragments.CategoriesFragment")]
    public class CategoriesFragment : BaseFragment<CategoryListViewModel> {
        protected override int FragmentId => Resource.Layout.fragment_category_list;
        protected override string Title => Strings.CategoriesLabel;
        public MvxCommand<Category> EditCategoryCommand => new MvxCommand<Category>(ShowEditCategoryDialog);

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var categoryList = view.FindViewById<MvxListView>(Resource.Id.category_list);
            categoryList.ItemClick = EditCategoryCommand;
            RegisterForContextMenu(categoryList);

            view.FindViewById<FloatingActionButton>(Resource.Id.fab_create_category).Click += (s, e) => {
                var dialog = new ModifyCategoryDialog {
                    ViewModel = Mvx.Resolve<ModifyCategoryDialogViewModel>()
                };

                dialog.Show(Activity.FragmentManager, Strings.AddCategoryTitle);
            };

            return view;
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
                    ShowEditCategoryDialog(selected);
                    return true;

                case 1:
                    ViewModel.DeleteCategoryCommand.Execute(selected);
                    return true;

                default:
                    return false;
            }
        }

        private void ShowEditCategoryDialog(Category selectedCategory) {
            new ModifyCategoryDialog(selectedCategory)
                .Show(Activity.FragmentManager, Strings.AddCategoryTitle);
        }
    }
}