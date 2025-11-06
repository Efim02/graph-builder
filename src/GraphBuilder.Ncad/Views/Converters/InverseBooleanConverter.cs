namespace GraphBuilder.Ncad.Views.Converters;

using System.Globalization;
using System.Windows.Data;

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
        {
            return false;
        }
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
        {
            return false;
        }
        return true;
    }
}