namespace GraphBuilder.Ncad.Views.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// Конвертер, позволяющий использовать несколько конвертеров друг за другом.
    /// </summary>
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        /// <summary>
        /// Конвертирует конвертеры друг за другом.
        /// </summary>
        /// <param name="value"> Список конвертеров. </param>
        /// <param name="targetType"> Тип. Не используется. </param>
        /// <param name="parameter"> Параметр. Не используется. </param>
        /// <param name="culture"> CultureInfo. Не используется. </param>
        /// <returns> Результат последовательного конвертирования. </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value,
                (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value"> The value that is produced by the binding target. </param>
        /// <param name="targetType"> The type to convert to. </param>
        /// <param name="parameter"> The converter parameter to use. </param>
        /// <param name="culture"> The culture to use in the converter. </param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}