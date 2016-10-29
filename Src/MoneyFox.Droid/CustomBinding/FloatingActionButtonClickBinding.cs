using System;
using Android.Views;
using Clans.Fab;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Droid.CustomBinding
{
    public class FloatingActionButtonClickBinding : MvxAndroidTargetBinding
    {
        private readonly FloatingActionButton floatingActionButton;
        private IMvxCommand command;

        public FloatingActionButtonClickBinding(FloatingActionButton floatingActionButton) : base(floatingActionButton)
        {
            this.floatingActionButton = floatingActionButton;
        }

        public override Type TargetType => typeof(IMvxCommand);

        protected override void SetValueImpl(object target, object value)
        {
            command = (IMvxCommand)value;
            floatingActionButton.Click +=FloatingActionButtonOnClick;
        }

        private void FloatingActionButtonOnClick(object sender, EventArgs eventArgs)
        {
            var menu = (FloatingActionMenu)(sender as View).Parent;
            menu.Toggle(true);

            command.Execute();
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;
    }
}