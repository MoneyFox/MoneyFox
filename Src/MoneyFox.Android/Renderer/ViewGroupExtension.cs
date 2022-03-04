namespace MoneyFox.Droid.Renderer
{
    using AView = Android.Views.View;
    using AViewGroup = Android.Views.ViewGroup;
    using System.Collections.Generic;

    public static class ViewGroupExtension
    {
        internal static IEnumerable<T> GetChildrenOfType<T>(this AViewGroup self)
            where T : AView
        {
            for(int i = 0; i < self.ChildCount; i++)
            {
                AView? child = self.GetChildAt(i);
                if(child == null)
                {
                    continue;
                }

                if(child is T typedChild)
                {
                    yield return typedChild;
                }

                if(child is AViewGroup childAsViewGroup)
                {
                    IEnumerable<T> myChildren = childAsViewGroup.GetChildrenOfType<T>();
                    foreach(T nextChild in myChildren)
                    {
                        yield return nextChild;
                    }
                }
            }
        }
    }
}