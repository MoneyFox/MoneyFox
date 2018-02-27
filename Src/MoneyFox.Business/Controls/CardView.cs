using Xamarin.Forms;

namespace MoneyFox.Business.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            Padding = 0;
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.UWP)
            {
                HasShadow = false;
                OutlineColor = Color.Transparent;
                BackgroundColor = Color.Transparent;
            }
        }
    }
}
