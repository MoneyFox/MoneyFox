namespace MoneyFox.Presentation.ViewModels
{
    public class TagViewModel : BaseViewModel
    {
        private int id;
        private string name;

        public int Id
        {
            get => id;
            set {
                if (id == value) return;
                id = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set {
                if (name == value) return;
                name = value;
                RaisePropertyChanged();
            }
        }
    }
}
