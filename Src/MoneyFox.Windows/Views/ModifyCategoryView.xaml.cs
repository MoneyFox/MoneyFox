using MoneyFox.Shared.Model;
using MoneyFox.Shared.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace MoneyFox.Windows.Views
{
    /// <summary>
    /// Modify or create a category
    /// </summary>
    public sealed partial class ModifyCategoryView
    {
        public ModifyCategoryView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<ModifyCategoryViewModel>();

            // code to handle bottom app bar when keyboard appears
            // workaround since otherwise the keyboard would overlay some controls
            InputPane.GetForCurrentView().Showing +=
                (s, args) => { BottomCommandBar.Visibility = Visibility.Collapsed; };
            InputPane.GetForCurrentView().Hiding += (s, args2) =>
            {
                if (BottomCommandBar.Visibility == Visibility.Collapsed)
                {
                    BottomCommandBar.Visibility = Visibility.Visible;
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = (ModifyCategoryViewModel) DataContext;

            var category = e.Parameter as Category;
            if (category != null)
            {
                viewModel.IsEdit = true;
                viewModel.SelectedCategory = category;
            }
            else
            {
                viewModel.IsEdit = false;
                viewModel.SelectedCategory = new Category();
            }

            base.OnNavigatedTo(e);
        }
    }
}
