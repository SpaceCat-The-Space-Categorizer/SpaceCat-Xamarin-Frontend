using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class BoolToToolColorConverter : IValueConverter
    {
        public (string, string) HexToolColors = ("#808080", "#6EE03E" );

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool toolOn = (bool)value;
            if (toolOn)
                return Color.FromHex(HexToolColors.Item2);
            else
                return Color.FromHex(HexToolColors.Item1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
