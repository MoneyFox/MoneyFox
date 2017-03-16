using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MikePhil.Charting.Buffer
{
    public partial class BarBuffer
    {
        public override void Feed(Java.Lang.Object p0)
        {
            Feed(p0 as global::MikePhil.Charting.Interfaces.Datasets.IBarDataSet);
        }
    }
}