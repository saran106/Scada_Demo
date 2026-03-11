using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Scada_Demo.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string colors)
            {
                var colorArray = colors.Split(',');
                if (colorArray.Length == 2)
                {
                    string colorString = boolValue ? colorArray[0] : colorArray[1];
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorString));
                }
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}