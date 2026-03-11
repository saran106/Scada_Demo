using System;
using System.Globalization;
using System.Windows.Data;

namespace Scada_Demo.Converters
{
    public class LevelToTopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value = Agg1Level (1200)
            // parameter = "280" (max height of container)

            if (value is int level && parameter is string maxHeightStr)
            {
                if (double.TryParse(maxHeightStr, out double maxHeight))
                {
                    double maxLevel = 5000; // Maximum possible level

                    // First calculate height same as before
                    double height = (level / maxLevel) * maxHeight;
                    height = Math.Max(20, Math.Min(maxHeight, height));

                    // Then calculate TOP position
                    // Container top = 120, so top = 120 + (maxHeight - height)
                    // Example: If height = 180px, maxHeight = 280px
                    // Then top = 120 + (280 - 180) = 120 + 100 = 220
                    return 120 + (maxHeight - height);
                }
            }
            return 120.0; // Default top position
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}