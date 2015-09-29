// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the LinkerPleaseInclude type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.IoC;

namespace MoneyManager.Droid
{
    /// <summary>
    ///     Defines the LinkerPleaseInclude type.
    ///     This class is never actually executed, but when Xamarin linking is enabled it does how to
    ///     ensure types and properties are preserved in the deployed app.
    /// </summary>
    public class LinkerPleaseInclude
    {
        /// <summary>
        ///     Includes the MvxPropertyInjector.
        /// </summary>
        /// <param name="injector">MvxPropertyInjector.</param>
        public void Include(MvxPropertyInjector injector)
        {
            injector = new MvxPropertyInjector();
        }

        /// <summary>
        ///     Includes the INotifyPropertyChanged.
        /// </summary>
        /// <param name="changed">INotifyPropertyChanged.</param>
        public void Include(INotifyPropertyChanged changed)
        {
            changed.PropertyChanged += (sender, e) => { var test = e.PropertyName; };
        }

        /// <summary>
        ///     Includes the specified button.
        /// </summary>
        /// <param name="button">The button.</param>
        public void Include(Button button)
        {
            button.Click += (s, e) => button.Text = button.Text + string.Empty;
        }

        /// <summary>
        ///     Includes the specified check box.
        /// </summary>
        /// <param name="checkBox">The check box.</param>
        public void Include(CheckBox checkBox)
        {
            checkBox.CheckedChange += (sender, args) => checkBox.Checked = !checkBox.Checked;
        }

        /// <summary>
        ///     Includes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        public void Include(View view)
        {
            view.Click += (s, e) => view.ContentDescription = view.ContentDescription + string.Empty;
        }

        /// <summary>
        ///     Includes the specified textView.
        /// </summary>
        /// <param name="textView">The textView.</param>
        public void Include(TextView textView)
        {
            textView.TextChanged += (sender, args) => textView.Text = string.Empty + textView.Text;
            textView.Hint = string.Empty + textView.Hint;
        }

        /// <summary>
        ///     Includes the specified compoundButton.
        /// </summary>
        /// <param name="compoundButton">The compoundButton.</param>
        public void Include(CompoundButton compoundButton)
        {
            compoundButton.CheckedChange += (sender, args) => compoundButton.Checked = !compoundButton.Checked;
        }

        /// <summary>
        ///     Includes the specified seekBar.
        /// </summary>
        /// <param name="seekBar">The seekBar.</param>
        public void Include(SeekBar seekBar)
        {
            seekBar.ProgressChanged += (sender, args) => seekBar.Progress = seekBar.Progress + 1;
        }

        /// <summary>
        ///     Includes the specified changed.
        /// </summary>
        /// <param name="changed">The changed.</param>
        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged +=
                (s, e) => { var test = $"{e.Action}{e.NewItems}{e.NewStartingIndex}{e.OldItems}{e.OldStartingIndex}"; };
        }

        public void Include(LinearLayout layout)
        {
            layout.Click += (s, e) => layout.Visibility = layout.Visibility - 1;
        }

        /// <summary>
        ///     Includes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) =>
            {
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            };
        }
    }
}