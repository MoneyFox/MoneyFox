using Xamarin.Forms;

namespace MoneyFox.Business.Behavior
{
    /// <summary>
    ///     Executes the search command on text change
    /// </summary>
    public class TextChangedBehavior : Behavior<SearchBar>
    {
        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Bindable_TextChanged;
        }

        protected override void OnDetachingFrom(SearchBar bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= Bindable_TextChanged;
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((SearchBar) sender).SearchCommand?.Execute(e.NewTextValue);
        }
    }
}