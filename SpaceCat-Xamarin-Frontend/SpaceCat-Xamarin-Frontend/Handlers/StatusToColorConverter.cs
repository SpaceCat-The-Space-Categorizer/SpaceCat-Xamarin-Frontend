using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class StatusToColorConverter : IValueConverter
    {
        public string[] HexSeatStatusColors = { "#33C70039", "#3328B463", "#33FFC300" };

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
