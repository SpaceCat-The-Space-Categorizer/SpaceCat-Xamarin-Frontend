using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class StatusToColorConverter : IValueConverter
    {
        public string[] HexSeatStatusColors = { "#33FF3333", "#33CCDF3E", "#33FFE12B" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (string)value;
            if (status != null)
            {
                if (status == "Uncounted")
                {
                    return Color.FromHex(HexSeatStatusColors[0]);
                }
                else if (status == "Counted")
                {
                    return Color.FromHex(HexSeatStatusColors[1]);
                }
                else if (status == "InProgress")
                {
                    return Color.FromHex(HexSeatStatusColors[2]);
                }
            }
            return Color.FromHex(HexSeatStatusColors[0]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Uncounted";
        }
    }
}
