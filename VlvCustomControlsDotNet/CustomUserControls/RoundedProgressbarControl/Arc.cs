using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;

namespace VlvCustomControlsDotNet
{
    public class Arc : Shape
    {
        #region DP

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc),
                new UIPropertyMetadata(0.0, UpdateArc));

        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        protected override Geometry DefiningGeometry => throw new NotImplementedException();


        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc),
                new UIPropertyMetadata(90.0, UpdateArc));

        #endregion

        public Arc()
        {

        }

        internal void Recalculate()
        {
            //
        }

        private void GetAngleData()
        {
            //
        }

        private static void UpdateArc(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //
        }

    }
}
