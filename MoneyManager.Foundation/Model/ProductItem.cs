using Windows.UI.Xaml;

namespace MoneyManager.Foundation.Model {
	public class ProductItem {
		public string ImgLink { get; set; }
		public string Status { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Key { get; set; }
		public Visibility BuyNowButtonVisible { get; set; }
	}
}