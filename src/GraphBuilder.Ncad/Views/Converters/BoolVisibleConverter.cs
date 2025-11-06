namespace GraphBuilder.Ncad.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Конвертер значений из <see cref="bool" /> в <see cref="Visibility" />.
    /// </summary>
    public class BoolToVisible : IValueConverter
    {
        /// <summary>
        /// True - конвертер будет возвращать невидимость для случая true.
        /// </summary>
        public bool Not { get; set; }

        /// <summary>
        /// Конвертация значения из <see cref="bool" /> в <see cref="Visibility" />
        /// </summary>
        /// <param name="value"> Входное значение. </param>
        /// <param name="targetType"> Тип входного значения. </param>
        /// <param name="parameter"> Параметр флага инверсии. </param>
        /// <param name="culture"> Культура. </param>
        /// <returns>
        /// Возвращает <see cref="Visibility.Visible" />, если входное значение - true,
        /// а isInverseParameter не равен true. Иначе - <see cref="Visibility.Collapsed" />.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
                return DependencyProperty.UnsetValue;

            if (IsInverse(parameter))
                boolValue = !boolValue;

            return boolValue ^ Not
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        /// <summary>
        /// Конвертация значения из <see cref="Visibility" /> в <see cref="bool" />
        /// </summary>
        /// <param name="value"> Входное значение. </param>
        /// <param name="targetType"> Тип входного значения. </param>
        /// <param name="parameter"> Параметр флага инверсии. </param>
        /// <param name="culture"> Культура. </param>
        /// <returns>
        /// Возвращает true, если входное значение - <see cref="Visibility.Visible" />,
        /// а isInverseParameter не равен true. Иначе - false.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility visibility))
                return DependencyProperty.UnsetValue;

            var isInverse = IsInverse(parameter);
            var result = isInverse ? visibility != Visibility.Visible : visibility == Visibility.Visible;

            return result ^ Not;
        }

        /// <summary>
        /// Проверить, задан ли параметр инверсии.
        /// </summary>
        /// <param name="isInverseParameter"> Параметр значения инверсии </param>
        /// <returns> Возвращает true, если параметр задан и равен true. </returns>
        private static bool IsInverse(object isInverseParameter)
        {
            if (isInverseParameter is bool isInverse)
                return isInverse;

            if (isInverseParameter is string isInverseString && bool.TryParse(isInverseString, out isInverse))
                return isInverse;

            return false;
        }
    }
}