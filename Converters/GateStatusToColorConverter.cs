using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Scada_Demo.Converters
{
    public class GateStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status == "OPEN")
                    return new SolidColorBrush(Colors.Green);
                else
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B22222"));
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}