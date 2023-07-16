namespace MoneyFox.Ui.Controls;

#if IOS
using Microsoft.Maui.Platform;
using Platforms.iOS.Utils;
using UIKit;
#endif

public class FixedScrollView : ScrollView
{
    public FixedScrollView()
    {
#if IOS
        UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShowing!);
        UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHiding!);
#endif
    }

#if IOS
    private void OnKeyboardShowing(object sender, UIKeyboardEventArgs args)
    {
        if (Shell.Current.CurrentPage is ContentPage page)
        {
            var control = this.ToPlatform(Handler!.MauiContext!).FindFirstResponder();
            var rootUiView = page.Content.ToPlatform(Handler.MauiContext!);
            var kbFrame = UIKeyboard.FrameEndFromNotification(args.Notification);
            var distance = control.GetOverlapDistance(rootView: rootUiView, keyboardFrame: kbFrame);
            if (distance > 0)
            {
                Margin = new(left: Margin.Left, top: -distance, right: Margin.Right, bottom: distance);
            }
        }
    }

    private void OnKeyboardHiding(object sender, UIKeyboardEventArgs args)
    {
        Margin = new(left: Margin.Left, top: 0, right: Margin.Right, bottom: 0);
    }
#endif
}
