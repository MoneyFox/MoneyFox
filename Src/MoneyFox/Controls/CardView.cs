using Xamarin.Forms;

namespace MoneyFox.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            Padding = 0;
            OutlineColor = Color.LightGray;

            if (Device.RuntimePlatform == Device.iOS)
            {
                HasShadow = false;
                OutlineColor = Color.Transparent;
                BackgroundColor = Color.Transparent;
            }
        }

        public static readonly BindableProperty ShadowRadiusProperty = BindableProperty.Create(
            propertyName: "ShadowRadius",
            returnType: typeof(int),
            declaringType: typeof(CardView),
            defaultValue: 10);

        public int ShadowRadius
        {
            get => (int)GetValue(ShadowRadiusProperty);
            set => SetValue(ShadowRadiusProperty, value);
        }
    }
}
