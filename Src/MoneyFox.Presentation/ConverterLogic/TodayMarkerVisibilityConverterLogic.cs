using System;

namespace MoneyFox.Presentation.ConverterLogic
{
    public class TodayMarkerVisibilityConverterLogic
    {
        public static bool ShowMarker(DateTime dateTime) {
            return dateTime.Date == DateTime.Today;
        }
    }
}
