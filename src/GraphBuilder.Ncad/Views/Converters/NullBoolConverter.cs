namespace GraphBuilder.Ncad.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converter - для возвращения TRUE значения если Binding ссылается на NULL, иначе - FALSE.
    /// </summary>
    public class NullBoolConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Для реализации подобного конвертирования, необходимы доп. свойства.");
        }
    }
}