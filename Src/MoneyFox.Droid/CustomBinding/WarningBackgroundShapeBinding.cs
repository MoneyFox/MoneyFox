using System;
using Android.Widget;
using MoneyFox.Shared.Model;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace MoneyFox.Droid.CustomBinding
{
    public class WarningBackgroundShapeBinding : MvxAndroidTargetBinding
    {
        private readonly LinearLayout linearLayout;

        public WarningBackgroundShapeBinding(LinearLayout view) : base(view)
        {
            linearLayout = view;
        }

        protected override void SetValueImpl(object target, object value)
        {
            // to do logic
        }

        public override void SetValue(object value)
        {
            var input = (AccountViewModel)value;
            linearLayout.SetBackgroundResource(input.IsOverdrawn ? Resource.Color.color_warning : Resource.Color.white );
        }

        public override Type TargetType => typeof(AccountViewModel);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneTime;
    }
}