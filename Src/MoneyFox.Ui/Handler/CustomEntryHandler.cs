#if IOS
namespace MoneyFox.Ui.Handler;

using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

/// <summary>
/// An EntryHandler that overrides the MAUI Platform Entry Handler to better handle Entry focus events.
/// When this Handler is registered with the MAUI app, all Entries will automatically scroll the page to ensure the
/// focused Entry rests above the keyboard
/// </summary>
public class CustomEntryHandler : Microsoft.Maui.Handlers.EntryHandler
{
    private NSObject keyboardShowObserver;
    private NSObject keyboardHideObserver;

    protected override void ConnectHandler(MauiTextField platformView)
    {
        base.ConnectHandler(platformView);

        //iOS has dedicated keyboard notifications that fire whenever the keyboard will show and will hide.
        // Here, we are registering events to fire when the keyboard is shown or hidden
        keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
        keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
    }

    private void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
    {
        //If the MainPage is null for some reason, do not continue
        if (Application.Current?.MainPage is null)
            return;

        //If the entry that called this method is not the currently focused entry, do not scroll the page
        if (!PlatformView.Focused)
            //return;

        //Some devices have a pixel density for some reason. We want to convert the screen height into pixels by
        // dividing the MainDisplay Height by the pixel density (to get actual pixels)
        var density = DeviceDisplay.Current.MainDisplayInfo.Density;
        var mainDisplay = DeviceDisplay.Current.MainDisplayInfo.Height / density;

        //Grab the height of the keyboard. NOTE: This is the HEIGHT of the keyboard, which always appears from the bottom
        // At this time, it is safe to say that the height of the keyboard is also the ending Y pixel (when measured from
        // the bottom of the screen)
        var keyboardHeight = args.FrameEnd.Height;

        //Grab the Y position of the focused entry. Just to make your life more complicated, this value is calculated 
        // from the TOP. So we need to subtract the Y position of the entry from the amount of vertical pixels in the display
        // to get the ending Y coordinate for the Entry.
        var entryYPosition = mainDisplay - PlatformView.AccessibilityFrame.Y;

        //Grab the height of the focused Entry. This is important because we want to scroll the page to ensure the 
        // entire entry is visible. Without this value, the page would scroll to the top border of the entry (hiding the
        // main content under the keyboard).
        var entryHeight = PlatformView.Frame.Height;


        //If the keyboard height is greater than the Y position of the entry, the keyboard is about to cover the entry
        if (keyboardHeight >= entryYPosition)
        {
            //Determine how many pixels to scroll. Add some extra padding just for appearances
            var scrollAmount = keyboardHeight - entryYPosition + entryHeight + 10;
            Application.Current.MainPage.TranslationY = scrollAmount * -1;
        }
    }

    private void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
    {
        if (Application.Current?.MainPage is null)
            return;

        //When the keyboard is dismissed, reset the scroll of the page
        Application.Current.MainPage.TranslationY = 0;
    }

    //There is a problem with .NET MAUI at the time of writing this control: DisconnectHandler is never called
    // This means that every entry in the entire app is always listening for keyboard events. This is less than ideal.
    // If performance begins to take a hit, consider an alternative approach.
    protected override void DisconnectHandler(MauiTextField platformView)
    {
        if (keyboardShowObserver != null)
        {
            keyboardShowObserver.Dispose();
            keyboardShowObserver = null;
        }

        if (keyboardHideObserver != null)
        {
            keyboardHideObserver.Dispose();
            keyboardHideObserver = null;
        }

        base.DisconnectHandler(platformView);
    }
}
#endif
