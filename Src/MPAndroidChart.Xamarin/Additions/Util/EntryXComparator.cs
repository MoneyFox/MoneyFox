using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MikePhil.Charting.Util
{
    public partial class EntryXComparator
    {
        public unsafe int Compare(Java.Lang.Object p0, Java.Lang.Object p1)
        {
            return Compare(p0 as MikePhil.Charting.Data.Entry, p1 as MikePhil.Charting.Data.Entry);
        }
    }
}