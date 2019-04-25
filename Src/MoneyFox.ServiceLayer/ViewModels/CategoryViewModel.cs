using GenericServices;
using MoneyFox.DataLayer.Entities;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class CategoryViewModel : ViewModelBase, ILinkToEntity<Category>
    {
        private int id;
        private string name;
        private string note;

        public int Id
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }


        /// <summary>
        ///     Additional details about the CategoryViewModel
        /// </summary>
        public string Note
        {
            get => note;
            set => this.RaiseAndSetIfChanged(ref note, value);
        }
    }
}