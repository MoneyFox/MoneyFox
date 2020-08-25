using Microsoft.Toolkit.Uwp.UI.Controls;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Ui.Shared.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class PaymentListView: INotifyPropertyChanged
    {
        public override bool ShowHeader => false;

        private PaymentListViewModel ViewModel => (PaymentListViewModel) DataContext;

        public PaymentListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                ViewModel.AccountId = (int)e.Parameter;
                ViewModel.InitializeCommand.Execute(null);
            }
        }

        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
        }

        private ICollectionView _groupView;
        public ICollectionView GroupView
        {
            get
            {
                return _groupView;
            }
            set
            {
                Set(ref _groupView, value);
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            List<DateListGroupCollection<PaymentViewModel>> groupedPayments = DateListGroupCollection<PaymentViewModel>
                .CreateGroups(ViewModel.Payments,
                                s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                                s => s.Date);

            var cvs = new CollectionViewSource();
            cvs.IsSourceGrouped = true;
            cvs.Source = groupedPayments;

            GroupView = cvs.View;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
