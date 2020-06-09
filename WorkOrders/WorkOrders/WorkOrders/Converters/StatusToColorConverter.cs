using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WorkOrders.Models;
using Xamarin.Forms;

namespace WorkOrders.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WorkOrderStatus status)
            {
                switch (status)
                {
                    case WorkOrderStatus.Available:
                        return Color.LightBlue;
                    case WorkOrderStatus.NeedsWork:
                        return Color.Yellow;
                    case WorkOrderStatus.Expired:
                        return Color.OrangeRed;
                    case WorkOrderStatus.Started:
                        return Color.DarkBlue;
                    case WorkOrderStatus.Overdue:
                        return Color.DarkRed;
                    case WorkOrderStatus.Done:
                        return Color.Green;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
