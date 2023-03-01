namespace MoneyFox.Ui.Controls;

#if IOS
using Microsoft.Maui.Platform;
using UIKit;
using CoreGraphics;
using MoneyFox.Ui.Platforms.iOS.Utils;
#endif

public class FixedScrollView : ScrollView
{

    public FixedScrollView()
    {
#if __IOS__
        UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShowing!);
        UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHiding!);
#endif
    }

#if __IOS__
    private void OnKeyboardShowing(object sender, UIKeyboardEventArgs args)
    {
        if (Shell.Current.CurrentPage is ContentPage page)
        {
            UIView control = this.ToPlatform(Handler!.MauiContext!).FindFirstResponder();
            UIView rootUiView = page.Content.ToPlatform(Handler.MauiContext!);
            CGRect kbFrame = UIKeyboard.FrameEndFromNotification(args.Notification);
            double distance = control.GetOverlapDistance(rootUiView, kbFrame);
            if (distance > 0)
            {
                Margin = new Thickness(Margin.Left, -distance, Margin.Right, distance);
            }
        }
    }

    private void OnKeyboardHiding(object sender, UIKeyboardEventArgs args)
    {
        Margin = new Thickness(Margin.Left, 0, Margin.Right, 0);
    }
#endif
}
