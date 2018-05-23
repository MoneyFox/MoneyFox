using System;
using Android.Content;
using FAB = Android.Support.Design.Widget.FloatingActionButton;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content.Res;
using System.ComponentModel;
using MoneyFox.Controls;
using MoneyFox.Droid.Renderer;

[assembly: ExportRenderer(typeof(FloatingActionButton), typeof(FloatingActionButtonRenderer))]

namespace MoneyFox.Droid.Renderer {
    public class FloatingActionButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<FloatingActionButton, FAB> {
        public static void Initialize() {
            /* used only for helping ensure the renderer is not linked out */
        }

        public FloatingActionButtonRenderer(Context context) : base(context) {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButton> e) {
            base.OnElementChanged(e);


            if (e.NewElement == null)

                return;


            var fab = new FAB(Context);

            // set the bg

            Android.Support.V4.View.ViewCompat.SetBackgroundTintList(fab,
                ColorStateList.ValueOf(Element.ButtonColor.ToAndroid()));

            fab.UseCompatPadding = true;


            // set the icon

            var elementImage = Element.Image;

            var imageFile = elementImage?.File;


            if (imageFile != null) {
                fab.SetImageDrawable(Context.Resources.GetDrawable(imageFile));
            }

            fab.Click += Fab_Click;

            SetNativeControl(fab);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b) {
            base.OnLayout(changed, l, t, r, b);

            Control.BringToFront();
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            var fab = (FAB) Control;

            if (e.PropertyName == nameof(Element.ButtonColor)) {
                Android.Support.V4.View.ViewCompat.SetBackgroundTintList(fab,
                    ColorStateList.ValueOf(Element.ButtonColor.ToAndroid()));
            }

            if (e.PropertyName == nameof(Element.Image)) {
                var elementImage = Element.Image;

                var imageFile = elementImage?.File;


                if (imageFile != null) {
                    fab.SetImageDrawable(Context.Resources.GetDrawable(imageFile));
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }


        private void Fab_Click(object sender, EventArgs e) {
            // proxy the click to the element

            ((IButtonController) Element).SendClicked();
        }
    }
}