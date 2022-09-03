using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TVCustomControlLibrary.Enums;

namespace TVCustomControlLibrary
{
    public class ProgressBarShape : Shape
    {
        private RectangleGeometry _rectangle;
        private EllipseGeometry _ellipse;
        private Size _final;

        public static DependencyProperty ShapeProperty =
            DependencyProperty.Register(nameof(Shape), typeof(ShapeType), typeof(ProgressBarShape), null);

        public ShapeType Shape
        {
            get => (ShapeType)GetValue(ShapeProperty);
            set => SetValue(ShapeProperty, value);
        }

        public ProgressBarShape()
        {
        }

        protected override Geometry DefiningGeometry => GetGeometry();

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        private Geometry GetGeometry()
        {
            if (Shape == ShapeType.Ellipse)
            {
                if (_ellipse != null)
                    return _ellipse;

                _ellipse = new EllipseGeometry(new Point(RenderSize.Width / 2, RenderSize.Height / 2), RenderSize.Width / 2, RenderSize.Height / 2);
                return _ellipse;
            }
            else if (Shape == ShapeType.Rectangle)
            {
                if (_rectangle != null)
                    return _rectangle;

                Rect rect = new Rect();
                rect.Width = RenderSize.Width;
                rect.Height = RenderSize.Height;
                _rectangle = new RectangleGeometry(rect);
                return _rectangle;
            }
            else
            {
                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure { IsClosed = true, StartPoint = new Point(10, 10), };

                pathFigure.Segments.Add(new LineSegment
                {
                    Point = new Point(10, 20),
                });
                pathFigure.Segments.Add(new LineSegment
                {
                    Point = new Point(10, 40),
                });
                pathGeometry.Figures.Add(pathFigure);

                return pathGeometry;

                //DrawingVisual triangle = new DrawingVisual();
                //using (DrawingContext dc = triangle.RenderOpen())
                //{
                //    var start = new Point(0, 50);

                //    var segments = new[]
                //    {
                //        new LineSegment(new Point(50,0), true),
                //        new LineSegment(new Point(50, 100), true)
                //    };

                //    var figure = new PathFigure(start, segments, true);
                //    var geo = new PathGeometry(new[] { figure });
                //    dc.DrawGeometry(Brushes.Red, null, geo);

                //    var drawingPen = new Pen(Brushes.Black, 3);
                //    dc.DrawLine(drawingPen, new Point(0, 50), new Point(50, 0));
                //    dc.DrawLine(drawingPen, new Point(50, 0), new Point(50, 100));
                //    dc.DrawLine(drawingPen, new Point(50, 100), new Point(0, 50));
                //}
                //return triangleж
            }
        }

        internal void RecalculateBounds(double width, double height)
        {
            Width = width;
            Height = height;
            _final = new Size(width, height);
        }

        #region Measure and arrange of this. Bad approach, but its only method i found to work. Maybe can have downsides

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = base.MeasureOverride(constraint);
            _final = constraint;
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(_final);
            return _final;
        }

        #endregion
    }
}
