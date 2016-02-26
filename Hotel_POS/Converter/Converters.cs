using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Hotel_POS.Resource;

namespace Hotel_POS
{
    public class CurrencyConcatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TerminalCommon.currency + " " + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Empty;
        }
    }

    class Helper : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Always test MultiValueConverter inputs for non-null
            // (to avoid crash bugs for views in the designer)
            if (values[0] is bool && values[1] is bool)
            {
                bool hasText = !(bool)values[0];
                bool hasFocus = (bool)values[1];

                if (hasFocus || hasText)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
