namespace MoneyFox.Droid.Renderer
{

    using System.Collections.Generic;
    using AView = Android.Views.View;
    using AViewGroup = Android.Views.ViewGroup;

    public static class ViewGroupExtension
    {
        internal static IEnumerable<T> GetChildrenOfType<T>(this AViewGroup self) where T : AView
        {
            for (var i = 0; i < self.ChildCount; i++)
            {
                var child = self.GetChildAt(i);
                if (child == null)
                {
                    continue;
                }

                if (child is T typedChild)
                {
                    yield return typedChild;
                }

                if (child is AViewGroup childAsViewGroup)
                {
                    var myChildren = childAsViewGroup.GetChildrenOfType<T>();
                    foreach (var nextChild in myChildren)
                    {
                        yield return nextChild;
                    }
                }
            }
        }
    }

}
