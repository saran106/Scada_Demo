using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Scada_Demo.Converters
{
    public class GateStatusToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status == "OPEN")
                    return new SolidColorBrush(Colors.Green);
                else
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}