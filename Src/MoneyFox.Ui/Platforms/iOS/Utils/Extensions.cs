namespace MoneyFox.Ui.Platforms.iOS.Utils;

using System;
using CoreGraphics;
using UIKit;

// Helper extensions to compute how much shift the UI up
// when the keyboard shows to avoid hiding input fields.
// Inspired by Keyboard Overlap Xamarin Forms plugin:
// https://github.com/lelebs/Xamarin.Forms.Plugins/blob/master/KeyboardOverlap/KeyboardOverlap/KeyboardOverlap.Forms.Plugin.iOSUnified/KeyboardOverlapRenderer.cs
public static class UIViewExtensions
{
    /// <summary>
    ///     Find the first responder in the <paramref name="view" />'s subview hierarchy
    /// </summary>
    /// <param name="view">
    ///     A <see cref="UIView" />
    /// </param>
    /// <returns>
    ///     A <see cref="UIView" /> that is the first responder or null if there is no first responder
    /// </returns>
    public static UIView FindFirstResponder(this UIView view)
    {
        if (view.IsFirstResponder)
        {
            return view;
        }

        foreach (UIView subView in view.Subviews)
        {
            var firstResponder = subView.FindFirstResponder();

            if (firstResponder != null) return firstResponder;
        }

        return null;
    }

    /// <summary>
    ///     Returns the view Bottom (Y + Height) coordinates relative to the rootView
    /// </summary>
    /// <returns>The view relative bottom.</returns>
    /// <param name="view">View.</param>
    /// <param name="rootView">Root view.</param>
    private static double GetViewRelativeBottom(this UIView view, UIView rootView)
    {
        // https://developer.apple.com/documentation/uikit/uiview/1622424-convertpoint
        var viewRelativeCoordinates = rootView.ConvertPointFromView(new CGPoint(0, 0), view);
        var activeViewRoundedY = Math.Round(viewRelativeCoordinates.Y, 2);

        return activeViewRoundedY + view.Frame.Height;
    }

    private static double GetOverlapDistance(double relativeBottom, UIView rootView, CGRect keyboardFrame)
    {
        var safeAreaBottom = rootView.Window.SafeAreaInsets.Bottom;
        var pageHeight = rootView.Frame.Height;
        var keyboardHeight = keyboardFrame.Height;

        return relativeBottom - (pageHeight + safeAreaBottom - keyboardHeight);
    }

    /// <summary>
    ///     Returns the distance between the bottom of the View and the top of the keyboard
    /// </summary>
    /// <param name="activeView">View.</param>
    /// <param name="rootView">Root view.</param>
    /// <param name="keyboardFrame">Keyboard Frame.</param>
    /// <returns>The distance, positive if the keyboard overlaps with the View, negative otherwise</returns>
    public static double GetOverlapDistance(this UIView activeView, UIView rootView, CGRect keyboardFrame)
    {
        if(activeView == null)
        {
            return 0;
        }

        double bottom = activeView.GetViewRelativeBottom(rootView);

        return GetOverlapDistance(bottom, rootView, keyboardFrame);
    }
}
