using PropertyChanged;

namespace MoneyManager.DataAccess.Model
{
    [ImplementPropertyChanged]
    internal class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}