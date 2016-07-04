using System;
using System.Collections.Generic;
using System.Text;
using MoneyFox.Shared.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace MoneyFox.Ios.Views {
    /// <summary>
    /// A base view controller 
    /// </summary>
    public class BaseViewController<TViewModel> : MvxViewController where TViewModel : BaseViewModel {
        #region Fields

        protected bool NavigationBarEnabled = false;

        public new TViewModel ViewModel {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        #endregion

        #region Public Methods

        public override void ViewDidLoad() {
            EdgesForExtendedLayout = UIRectEdge.None;
            View.BackgroundColor = UIColor.White;

            base.ViewDidLoad();
        }

        #endregion
    }
}