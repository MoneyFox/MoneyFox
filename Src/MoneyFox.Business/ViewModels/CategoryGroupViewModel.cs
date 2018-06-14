using MoneyFox.DataAccess.Pocos;

namespace MoneyFox.Business.ViewModels
{    
    /// <summary>
    ///     Representation of an category CategoryGroup view.
    /// </summary>
    public class CategoryGroupViewModel : BaseViewModel
    {

        /// <summary>
        ///     Constructor. Takes a Catgegroup POCO.
        /// </summary>
        /// <param name="categoryGroup">Category Group POCO who contains the data.</param>
        public CategoryGroupViewModel(CategoryGroup categoryGroup)
        {
            this.CategoryGroup = categoryGroup;
        }

        public CategoryGroup CategoryGroup { get; set; }

        /// <summary>
        ///     Account Name
        /// </summary>
        public string Name
        {
            get => CategoryGroup.Data.Name;
            set
            {
                if (CategoryGroup.Data.Name == value) return;
                CategoryGroup.Data.Name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Note
        /// </summary>
        public string Note
        {
            get => CategoryGroup.Data.Note;
            set
            {
                if (CategoryGroup.Data.Note == value) return;
                CategoryGroup.Data.Note = value;
                RaisePropertyChanged();
            }
        }
    }
}
