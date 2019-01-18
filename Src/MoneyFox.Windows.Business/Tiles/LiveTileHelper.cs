namespace MoneyFox.Windows.Business.Tiles
{
    public static class LiveTileHelper
    {
        public static string TruncateNumber(double num)
        {
            if (num > 0 && num < 1000)
            {
                return num.ToString("#,0");
            }
            if (num > 1000 && num < 1000000)
            {
                double test = num / 1000;
                return test.ToString("#.0") + "K";
            }
            if (num > 1000000 && num < 1000000000)
            {
                double test = num / 1000000;
                return test.ToString("#.1") + "M";
            }
            if (num > 1000000000 & num < 1000000000000)
            {
                double test = num / 1000000000;
                return test.ToString("#.0") + "B";
            }
            return "Number out of Range";
        }
    }
}
