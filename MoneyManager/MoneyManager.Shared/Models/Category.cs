using PropertyChanged;

namespace MoneyManager.Models
{
    [ImplementPropertyChanged]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}