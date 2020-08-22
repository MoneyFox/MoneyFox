using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Extensions;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Views.Categories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Categories
{
    public class CategoryListViewModel : ViewModelBase
    {
        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> categories = new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>();

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CategoryListViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            MessengerInstance.Register<ReloadMessage>(this, async (m) => await InitializeAsync());
        }

        public async Task InitializeAsync()
        {
            var categorieVms = mapper.Map<List<CategoryViewModel>>(await mediator.Send(new GetCategoryBySearchTermQuery()));

            var groups = AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(categorieVms, CultureInfo.CurrentUICulture,
                s => string.IsNullOrEmpty(s.Name)
                ? "-"
                : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));

            Categories = new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>(groups);
        }

        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> Categories
        {
            get => categories;
            set
            {
                categories = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GoToAddCategoryCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddCategoryRoute));

        public RelayCommand<CategoryViewModel> GoToEditCategoryCommand
            => new RelayCommand<CategoryViewModel>(async (categoryViewModel)
                => await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new EditCategoryPage(categoryViewModel.Id)) { BarBackgroundColor = Color.Transparent }));
    }
}
