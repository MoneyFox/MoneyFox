using System;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Ios
{
	public class MenuModel
	{
		public String Title
		{
			get;
			set;
		}

		public String ImageName
		{
			get;
			set;
		}

		public IMvxCommand Navigate
		{
			get;
			set;
		}

		public MenuModel() { }
	}
}
