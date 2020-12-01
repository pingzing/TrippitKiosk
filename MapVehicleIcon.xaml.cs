using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrippitKiosk
{
    public sealed partial class MapVehicleIcon : UserControl
    {
        public static readonly DependencyProperty RotationDegreesProperty =
            DependencyProperty.Register(
                nameof(RotationDegrees),
                typeof(double),
                typeof(MapVehicleIcon),
                new PropertyMetadata(0, RotationDegreesChanged));

        private static void RotationDegreesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as MapVehicleIcon;
            double newRotation = (double)e.NewValue;
            _this.RotationTransform.Angle = newRotation;
        }

        public double RotationDegrees
        {
            get => (double)GetValue(RotationDegreesProperty);
            set => SetValue(RotationDegreesProperty, value);
        }

        public static readonly DependencyProperty RouteTextProperty =
            DependencyProperty.Register(
                nameof(RouteText),
                typeof(string),
                typeof(MapVehicleIcon),
                new PropertyMetadata(null, RouteTextChanged));

        private static void RouteTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as MapVehicleIcon;
            string newText = (string)e.NewValue;
            _this.RouteTextBlock.Text = newText;
        }

        public string RouteText
        {
            get => (string)GetValue(RouteTextProperty);
            set => SetValue(RouteTextProperty, value);
        }

        public MapVehicleIcon()
        {
            this.InitializeComponent();
        }
    }
}
