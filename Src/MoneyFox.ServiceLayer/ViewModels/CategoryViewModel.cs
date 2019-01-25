using GenericServices;
using MoneyFox.DataLayer.Entities;
using MvvmCross.ViewModels;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class CategoryViewModel : MvxViewModel, ILinkToEntity<Category>
    {
        private int id;
        private string name;
        private string note;

        public int Id
        {
            get => id;
            set
            {
                if (id == value) return;
                id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     Additional details about the CategoryViewModel
        /// </summary>
        public string Notes
        {
            get => note;
            set
            {
                if (note == value) return;
                note = value;
                RaisePropertyChanged();
            }
        }
    }
}