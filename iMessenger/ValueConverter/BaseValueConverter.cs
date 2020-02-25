using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace iMessenger
{
    /// <summary>
    /// A Base Value Conveerter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T">the type of this value conveter</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class , new()
    {
        #region Private Members
        
        /// <summary>
        /// A Single static instance of this value converter
        /// </summary>
        private static T mConverter = null;

        #endregion

        #region Markup extension Methods

        /// <summary>
        /// provides a static instance of the value converter
        /// </summary>
        /// <param name="serviceProvider">The servic provider </param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter ?? (mConverter = new T());
        }

        #endregion

        #region Value Converter Methods

        /// <summary>
        /// Method convert one type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Method convert value to its source type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
