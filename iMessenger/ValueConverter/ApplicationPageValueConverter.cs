using System;
using System.Globalization;
using System.Diagnostics;
using iMessenger.Pages;

namespace iMessenger
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Find appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.login:
                    return new LoginPage();

                case ApplicationPage.chat:
                    return new ChatPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
