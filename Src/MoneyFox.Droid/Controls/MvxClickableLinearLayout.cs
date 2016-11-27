using System.Windows.Input;
using Android.Content;
using Android.Util;
using MoneyFox.Droid.Adapter;
using MvvmCross.Binding.Droid.Views;

namespace MoneyFox.Droid.Controls
{
    public class MvxClickableLinearLayout : MvxLinearLayout
    {
        public MvxClickableLinearLayout(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxClickableLinearLayoutAdapter(context))
        {
        }

        public MvxClickableLinearLayout(Context context, IAttributeSet attrs, MvxClickableLinearLayoutAdapter adapter)
            : base(context, attrs, adapter)
        {
            var mvxClickableLinearLayoutAdapter = Adapter as MvxClickableLinearLayoutAdapter;
            if (mvxClickableLinearLayoutAdapter != null)
            {
                mvxClickableLinearLayoutAdapter.OnItemClick = OnItemClick;
                mvxClickableLinearLayoutAdapter.OnItemLongClick = OnItemLongClick;
            }
        }

        public ICommand ItemClick { get; set; }

        public ICommand ItemLongClick { get; set; }

        public void OnItemClick(object item)
        {
            if (ItemClick != null && ItemClick.CanExecute(item))
            {
                ItemClick.Execute(item);
            }
        }

        public void OnItemLongClick(object item)
        {
            if (ItemClick != null && ItemClick.CanExecute(item))
            {
                ItemClick.Execute(item);
            }
        }
    }
}