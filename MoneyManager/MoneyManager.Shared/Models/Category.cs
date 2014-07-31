using MoneyTracker.Src;
using PropertyChanged;

namespace MoneyTracker.Models
{
    [ImplementPropertyChanged]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}