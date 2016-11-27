using Android.Content;
using Android.Views;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform.Core;

namespace MoneyFox.Droid.Adapter
{
    public class MvxClickableLinearLayoutAdapter : MvxAdapterWithChangedEvent, View.IOnClickListener, View.IOnLongClickListener
    {
        public delegate void ItemClickDelegate(object item);
        public delegate void ItemLongClickDelegate(object item);

        public ItemClickDelegate OnItemClick;
        public ItemLongClickDelegate OnItemLongClick;

        public MvxClickableLinearLayoutAdapter(Context context)
            : base(context)
        {
        }

        public void OnClick(View view)
        {
            var mvxDataConsumer = view as IMvxDataConsumer;

            if (mvxDataConsumer != null)
            {
                OnItemClick?.Invoke(mvxDataConsumer.DataContext);
            }
        }

        public bool OnLongClick(View view)
        {
            var mvxDataConsumer = view as IMvxDataConsumer;

            if (mvxDataConsumer != null)
            {
                OnItemLongClick?.Invoke(mvxDataConsumer.DataContext);
            }
            return true;
        }

        protected override View GetView(int position, View convertView, ViewGroup parent, int templateId)
        {
            View view = base.GetView(position, convertView, parent, templateId);
            view.SetOnClickListener(this);
            view.SetOnLongClickListener(this);
            return view;
        }
    }
}