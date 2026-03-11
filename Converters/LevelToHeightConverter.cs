using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Scada_Demo.Converters
{
    // 1. Level to Height Converter
    public class LevelToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int level && parameter is string maxHeightStr)
            {
                if (double.TryParse(maxHeightStr, out double maxHeight))
                {
                    double maxLevel = 5000;
                    double height = (level / maxLevel) * maxHeight;
                    return Math.Max(20, Math.Min(maxHeight, height));
                }
            }
            return 50.0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }


}