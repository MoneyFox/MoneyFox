namespace MoneyFox.Ui.Controls;

public class NonOverlappingScrollView: ScrollView
{
#if __IOS__
    public NonOverlappingScrollView()
    {

        UIKit.UIKeyboard.Notifications.ObserveDidShow(OnKeyboardShow!);
        UIKit.UIKeyboard.Notifications.ObserveDidHide(OnKeyboardHide!);

    }

    private void OnKeyboardShow(object sender, UIKit.UIKeyboardEventArgs args) {
        var result = (Foundation.NSValue)args.Notification.UserInfo!.ObjectForKey(new Foundation.NSString(UIKit.UIKeyboard.FrameEndUserInfoKey));
        var keyboardSize = result.RectangleFValue.Size;
        Margin = new(Margin.Left, Margin.Top, Margin.Right, Margin.Bottom + keyboardSize.Height);
    }

    private void OnKeyboardHide(object sender, UIKit.UIKeyboardEventArgs args)
    {
        var result = (Foundation.NSValue)args.Notification.UserInfo!.ObjectForKey(new Foundation.NSString(UIKit.UIKeyboard.FrameEndUserInfoKey));
        var keyboardSize = result.RectangleFValue.Size;
        Margin = new(Margin.Left, Margin.Top, Margin.Right, Margin.Bottom - keyboardSize.Height);
    }
#endif
}
