using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using Xamarin;

namespace MoneyManager.Windows.Controls
{
    public sealed class SwipeableSplitView : SplitView
    {
        public SwipeableSplitView()
        {
            DefaultStyleKey = typeof (SwipeableSplitView);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PaneRoot = GetTemplateChild("PaneRoot") as Grid;
            overlayRoot = GetTemplateChild("OverlayRoot") as Grid;
            PanArea = GetTemplateChild("PanArea") as Rectangle;
            DismissLayer = GetTemplateChild("DismissLayer") as Rectangle;

            if (PaneRoot == null || overlayRoot == null || PanArea == null || DismissLayer == null)
            {
                throw new ArgumentException("Make sure you have copied the default style to Generic.xaml!!");
            }

            var rootGrid = paneRoot.Parent as Grid;

            if (rootGrid == null)
            {
                throw new ArgumentException("Make sure you have copied the default style to Generic.xaml!!");
            }

            OpenSwipeablePaneAnimation = rootGrid.Resources["OpenSwipeablePane"] as Storyboard;
            CloseSwipeablePaneAnimation = rootGrid.Resources["CloseSwipeablePane"] as Storyboard;

            if (OpenSwipeablePaneAnimation == null || CloseSwipeablePaneAnimation == null)
            {
                throw new ArgumentException("Make sure you have copied the default style to Generic.xaml!!");
            }

            // initialization
            OnDisplayModeChanged(null, null);

            RegisterPropertyChangedCallback(DisplayModeProperty, OnDisplayModeChanged);

            // disable ScrollViewer as it will prevent finger from panning
            if (Pane is ListView || Pane is ListBox)
            {
                ScrollViewer.SetVerticalScrollMode(Pane, ScrollMode.Disabled);
            }
        }

        #region native property change handlers

        private void OnDisplayModeChanged(DependencyObject sender, DependencyProperty dp)
        {
            switch (DisplayMode)
            {
                case SplitViewDisplayMode.Inline:
                case SplitViewDisplayMode.CompactOverlay:
                case SplitViewDisplayMode.CompactInline:
                    PanAreaInitialTranslateX = 0d;
                    overlayRoot.Visibility = Visibility.Collapsed;
                    break;

                case SplitViewDisplayMode.Overlay:
                    PanAreaInitialTranslateX = OpenPaneLength*-1;
                    overlayRoot.Visibility = Visibility.Visible;
                    break;
            }

            if (sender != null)
            {
                CloseSwipeablePane();
            }
        }

        #endregion

        #region DismissLayer tap event handlers

        private void OnDismissLayerTapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSwipeablePane();
        }

        #endregion

        #region private variables

        private Grid paneRoot;
        private Grid overlayRoot;
        private Rectangle panArea;
        private Rectangle dismissLayer;
        private CompositeTransform paneRootTransform;
        private CompositeTransform panAreaTransform;
        private Storyboard openSwipeablePane;
        private Storyboard closeSwipeablePane;

        private readonly IList<SelectorItem> menuItems = new List<SelectorItem>();
        private int toBeSelectedIndex;
        private double distancePerItem;
        private double startingDistance;

        #endregion

        #region properties

        // safely subscribe/unsubscribe manipulation events here
        internal Grid PaneRoot
        {
            get { return paneRoot; }
            set
            {
                if (paneRoot != null)
                {
                    paneRoot.ManipulationStarted -= OnManipulationStarted;
                    paneRoot.ManipulationDelta -= OnManipulationDelta;
                    paneRoot.ManipulationCompleted -= OnManipulationCompleted;
                }

                paneRoot = value;

                if (paneRoot != null)
                {
                    paneRoot.ManipulationStarted += OnManipulationStarted;
                    paneRoot.ManipulationDelta += OnManipulationDelta;
                    paneRoot.ManipulationCompleted += OnManipulationCompleted;
                }
            }
        }

        // safely subscribe/unsubscribe manipulation events here
        internal Rectangle PanArea
        {
            get { return panArea; }
            set
            {
                if (panArea != null)
                {
                    panArea.ManipulationStarted -= OnManipulationStarted;
                    panArea.ManipulationDelta -= OnManipulationDelta;
                    panArea.ManipulationCompleted -= OnManipulationCompleted;
                }

                panArea = value;

                if (panArea != null)
                {
                    panArea.ManipulationStarted += OnManipulationStarted;
                    panArea.ManipulationDelta += OnManipulationDelta;
                    panArea.ManipulationCompleted += OnManipulationCompleted;
                }
            }
        }

        // safely subscribe/unsubscribe manipulation events here
        internal Rectangle DismissLayer
        {
            get { return dismissLayer; }
            set
            {
                if (dismissLayer != null)
                {
                    dismissLayer.Tapped -= OnDismissLayerTapped;
                }

                dismissLayer = value;

                if (dismissLayer != null)
                {
                    dismissLayer.Tapped += OnDismissLayerTapped;
                }
            }
        }

        // safely subscribe/unsubscribe animation completed events here
        internal Storyboard OpenSwipeablePaneAnimation
        {
            get { return openSwipeablePane; }
            set
            {
                if (openSwipeablePane != null)
                {
                    openSwipeablePane.Completed -= OnOpenSwipeablePaneCompleted;
                }

                openSwipeablePane = value;

                if (openSwipeablePane != null)
                {
                    openSwipeablePane.Completed += OnOpenSwipeablePaneCompleted;
                }
            }
        }

        // safely subscribe/unsubscribe animation completed events here
        internal Storyboard CloseSwipeablePaneAnimation
        {
            get { return closeSwipeablePane; }
            set
            {
                if (closeSwipeablePane != null)
                {
                    closeSwipeablePane.Completed -= OnCloseSwipeablePaneCompleted;
                }

                closeSwipeablePane = value;

                if (closeSwipeablePane != null)
                {
                    closeSwipeablePane.Completed += OnCloseSwipeablePaneCompleted;
                }
            }
        }

        public bool IsSwipeablePaneOpen
        {
            get { return (bool) GetValue(IsSwipeablePaneOpenProperty); }
            set { SetValue(IsSwipeablePaneOpenProperty, value); }
        }

        public static readonly DependencyProperty IsSwipeablePaneOpenProperty =
            DependencyProperty.Register("IsSwipeablePaneOpen", typeof (bool), typeof (SwipeableSplitView),
                new PropertyMetadata(false, OnIsSwipeablePaneOpenChanged));

        private static void OnIsSwipeablePaneOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var splitView = (SwipeableSplitView) d;

            switch (splitView.DisplayMode)
            {
                case SplitViewDisplayMode.Inline:
                case SplitViewDisplayMode.CompactOverlay:
                case SplitViewDisplayMode.CompactInline:
                    splitView.IsPaneOpen = !splitView.IsPaneOpen;
                    break;

                case SplitViewDisplayMode.Overlay:
                    if (splitView.OpenSwipeablePaneAnimation == null || splitView.CloseSwipeablePaneAnimation == null)
                        return;
                    if ((bool) e.NewValue)
                    {
                        splitView.OpenSwipeablePane();
                    }
                    else
                    {
                        splitView.CloseSwipeablePane();
                    }
                    break;
            }
        }

        public double PanAreaInitialTranslateX
        {
            get { return (double) GetValue(PanAreaInitialTranslateXProperty); }
            set { SetValue(PanAreaInitialTranslateXProperty, value); }
        }

        public static readonly DependencyProperty PanAreaInitialTranslateXProperty =
            DependencyProperty.Register("PanAreaInitialTranslateX", typeof (double), typeof (SwipeableSplitView),
                new PropertyMetadata(0d));

        public double PanAreaThreshold
        {
            get { return (double) GetValue(PanAreaThresholdProperty); }
            set { SetValue(PanAreaThresholdProperty, value); }
        }

        public static readonly DependencyProperty PanAreaThresholdProperty =
            DependencyProperty.Register("PanAreaThreshold", typeof (double), typeof (SwipeableSplitView),
                new PropertyMetadata(36d));

        #endregion

        #region manipulation event handlers

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            panAreaTransform = PanArea.RenderTransform as CompositeTransform;
            paneRootTransform = PaneRoot.RenderTransform as CompositeTransform;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var x = panAreaTransform.TranslateX + e.Delta.Translation.X;

            // keep the pan within the bountry
            if (x < PanAreaInitialTranslateX || x > 0) return;

            // while we are panning the PanArea on X axis, let's sync the PaneRoot's position X too
            paneRootTransform.TranslateX = panAreaTransform.TranslateX = x;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (Math.Abs(e.Velocities.Linear.Y) > 0.1) return;

            var x = e.Velocities.Linear.X;

            // ignore a little bit velocity (+/-0.1)
            if (x <= -0.1)
            {
                CloseSwipeablePane();
            }
            else if (x > -0.1 && x < 0.1)
            {
                if (Math.Abs(panAreaTransform.TranslateX) > Math.Abs(PanAreaInitialTranslateX)/2)
                {
                    CloseSwipeablePane();
                }
                else
                {
                    OpenSwipeablePane();
                }
            }
            else
            {
                OpenSwipeablePane();
            }
        }

        #endregion

        #region animation completed event handlers

        private void OnOpenSwipeablePaneCompleted(object sender, object e)
        {
            DismissLayer.IsHitTestVisible = true;
        }

        private void OnCloseSwipeablePaneCompleted(object sender, object e)
        {
            DismissLayer.IsHitTestVisible = false;
        }

        #endregion

        #region private methods

        private void OpenSwipeablePane()
        {
            if (IsSwipeablePaneOpen)
            {
                try
                {
                    OpenSwipeablePaneAnimation.Begin();
                }
                catch (Exception ex)
                {
                    Insights.Report(ex, Insights.Severity.Error);
                }
            }
            else
            {
                IsSwipeablePaneOpen = true;
            }
        }

        private void CloseSwipeablePane()
        {
            if (!IsSwipeablePaneOpen)
            {
                try
                {
                    CloseSwipeablePaneAnimation.Begin();
                }
                catch (Exception ex)
                {
                    Insights.Report(ex, Insights.Severity.Error);
                }
            }
            else
            {
                IsSwipeablePaneOpen = false;
            }
        }

        #endregion
    }
}