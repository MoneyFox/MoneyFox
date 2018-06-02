using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MoneyFox.Droid.Services
{
    /// <summary>
    ///     Shows a Dialog to select the operation on edit.
    ///     On Android this is used as context menu.
    /// </summary>
    public class ModifyDialogService : IModifyDialogService
    {
        private TaskCompletionSource<ModifyOperation> tcs;

        private readonly List<string> itemsForEditList = new List<string>
        {
            Strings.EditLabel,
            Strings.DeleteLabel
        };

        public Task<ModifyOperation> ShowEditSelectionDialog()
        {
            tcs = new TaskCompletionSource<ModifyOperation>();

            var builder = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
            builder.SetTitle(Strings.ChooseLabel);
            builder.SetItems(itemsForEditList.ToArray(), OnSelectItemForCreation);
            builder.SetNegativeButton(Strings.CancelLabel, (d, t) => (d as Android.App.Dialog).Dismiss());
            builder.Show();

            return tcs.Task;
        }

        public void OnSelectItemForCreation(object sender, DialogClickEventArgs args)
        {
            var selected = itemsForEditList[args.Which];

            if (selected == Strings.EditLabel)
            {
                tcs.SetResult(ModifyOperation.Edit);
            }
            else if (selected == Strings.DeleteLabel)
            {
                tcs.SetResult(ModifyOperation.Delete);
            }
        }
    }
}