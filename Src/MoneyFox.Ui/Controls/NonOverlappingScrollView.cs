namespace MoneyFox.Ui.Controls;

#if __IOS__
using Foundation;
using UIKit;
#endif

public class NonOverlappingScrollView : ScrollView
{
#if __IOS__
    public NonOverlappingScrollView()
    {
        UIKeyboard.Notifications.ObserveDidShow(OnKeyboardShow!);
        UIKeyboard.Notifications.ObserveDidHide(OnKeyboardHide!);
    }

    private void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
    {
        var result = (NSValue)args.Notification.UserInfo!.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
        var keyboardSize = result.RectangleFValue.Size;
        Margin = new(left: Margin.Left, top: Margin.Top, right: Margin.Right, bottom: Margin.Bottom + keyboardSize.Height);
    }

    private void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
    {
        var result = (NSValue)args.Notification.UserInfo!.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
        var keyboardSize = result.RectangleFValue.Size;
        Margin = new(left: Margin.Left, top: Margin.Top, right: Margin.Right, bottom: Margin.Bottom - keyboardSize.Height);
    }
#endif
}
