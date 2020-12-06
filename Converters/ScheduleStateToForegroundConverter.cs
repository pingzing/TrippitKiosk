using System;
using TrippitKiosk.Viewmodels;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace TrippitKiosk.Converters
{
    public class ScheduleStateToForegroundConverter : IValueConverter
    {
        private static readonly SolidColorBrush GreenBrush = new SolidColorBrush(Colors.Green);
        private static readonly SolidColorBrush RedBrush = new SolidColorBrush(Colors.Red);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ScheduleState state = (ScheduleState)value;
            return state switch
            {
                ScheduleState.Normal => GreenBrush, // doesn't matter, the UI isn't visble when in a normal state
                ScheduleState.Ahead => GreenBrush,
                ScheduleState.Delayed => RedBrush,
                _ => throw new ArgumentException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
