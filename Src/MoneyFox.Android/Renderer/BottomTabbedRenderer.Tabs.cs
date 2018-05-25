using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Com.Ittianyu.Bottomnavigationviewex;
using MoneyFox.Droid.Utils;
using Xamarin.Forms;

namespace MoneyFox.Droid.Renderer {
    public partial class BottomTabbedRenderer : BottomNavigationViewEx.IOnNavigationItemSelectedListener {
        public static int? ItemBackgroundResource;

        public static ColorStateList ItemIconTintList;

        public static ColorStateList ItemTextColor;

        public static Android.Graphics.Color? BackgroundColor;

        public static Typeface Typeface;

        public static float? IconSize;

        public static float? FontSize;

        public static float ItemSpacing;

        public static ItemAlignFlags ItemAlign;

        public static Thickness ItemPadding;

        public static bool? VisibleTitle;


        public bool OnNavigationItemSelected(IMenuItem item) {
            this.SwitchPage(item);

            return true;
        }


        internal void SetupTabItems() {
            this.SetupTabItems(bottomNav);
        }


        internal void SetupBottomBar() {
            bottomNav = this.SetupBottomBar(rootLayout, bottomNav, barId);
        }
    }


    public enum ItemAlignFlags {
        Default,
        Center,
        Top,
        Bottom
    }
}