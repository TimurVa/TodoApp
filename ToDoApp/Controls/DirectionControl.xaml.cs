using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ToDoApp.Controls
{
    public partial class DirectionControl : UserControl
    {
        #region MajorTicksCount dp
        public int MajorTicksCount
        {
            get { return (int)GetValue(MajorTicksCountProperty); }
            set { SetValue(MajorTicksCountProperty, value); }
        }

        public static readonly DependencyProperty MajorTicksCountProperty =
            DependencyProperty.Register(nameof(MajorTicksCount), typeof(int), typeof(DirectionControl), new PropertyMetadata(4, propertyChangedCallback: new PropertyChangedCallback(OnMajorTicksCountChanged)));

        private static void OnMajorTicksCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DirectionControl)d).OnMajorTicksCountChanged(e);
        }

        private void OnMajorTicksCountChanged(DependencyPropertyChangedEventArgs e)
        {
            ReDraw();
        }
        #endregion


        #region StartAngleInDegree dp
        public double StartAngleInDegree
        {
            get { return (double)GetValue(StartAngleInDegreeProperty); }
            set { SetValue(StartAngleInDegreeProperty, value); }
        }

        public static readonly DependencyProperty StartAngleInDegreeProperty =
            DependencyProperty.Register(nameof(StartAngleInDegree), typeof(double), typeof(DirectionControl), new PropertyMetadata(0.0d, propertyChangedCallback: new PropertyChangedCallback(OnStartAngleInDegreeChanged)));

        private static void OnStartAngleInDegreeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DirectionControl)d).OnStartAngleInDegreeChanged(e);
        }

        private void OnStartAngleInDegreeChanged(DependencyPropertyChangedEventArgs e)
        {
            ReDraw();
        }
        #endregion


        #region EndAngleInDegree dp
        public double EndAngleInDegree
        {
            get { return (double)GetValue(EndAngleInDegreeProperty); }
            set { SetValue(EndAngleInDegreeProperty, value); }
        }

        public static readonly DependencyProperty EndAngleInDegreeProperty =
            DependencyProperty.Register(nameof(EndAngleInDegree), typeof(double), typeof(DirectionControl), new PropertyMetadata(360.0d, propertyChangedCallback: new PropertyChangedCallback(OnEndAngleInDegreeChanged)));

        private static void OnEndAngleInDegreeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DirectionControl)d).OnEndAngleInDegreeChanged(e);
        }

        private void OnEndAngleInDegreeChanged(DependencyPropertyChangedEventArgs e)
        {
            ReDraw();
        }
        #endregion


        //#region MajorTick dp
        //public double MajorTick
        //{
        //    get { return (double)GetValue(MajorTickProperty); }
        //    set { SetValue(MajorTickProperty, value); }
        //}

        //public static readonly DependencyProperty MajorTickProperty =
        //    DependencyProperty.Register(nameof(MajorTick), typeof(double), typeof(DirectionControl), new PropertyMetadata(90.0d, propertyChangedCallback: new PropertyChangedCallback(OnMajorTickPropertyChanged)));

        //private static void OnMajorTickPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((DirectionControl)d).OnMajorTickPropertyChanged(e);
        //}

        //private void OnMajorTickPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    ReDraw();
        //}
        //#endregion


        #region MinorTickCount dp
        public int MinorTickCount
        {
            get { return (int)GetValue(MinorTickCountProperty); }
            set { SetValue(MinorTickCountProperty, value); }
        }

        public static readonly DependencyProperty MinorTickCountProperty =
            DependencyProperty.Register(nameof(MinorTickCount), typeof(int), typeof(DirectionControl), new PropertyMetadata(5, propertyChangedCallback: new PropertyChangedCallback(OnMinorTickCountChanged)));

        private static void OnMinorTickCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DirectionControl)d).OnMinorTickCountChanged(e);
        }

        private void OnMinorTickCountChanged(DependencyPropertyChangedEventArgs e)
        {
            ReDraw();
        }
        #endregion


        #region Stroke dp
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(DirectionControl), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#48484a"))));
        #endregion


        #region Fill dp
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(DirectionControl), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ebebf0"))));
        #endregion


        #region Minor tick brush dp
        public Brush MinorTickBrush
        {
            get { return (Brush)GetValue(MinorTickBrushProperty); }
            set { SetValue(MinorTickBrushProperty, value); }
        }

        public static readonly DependencyProperty MinorTickBrushProperty =
            DependencyProperty.Register(nameof(MinorTickBrush), typeof(Brush), typeof(DirectionControl), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0a7aff"))));
        #endregion


        #region Major tick color dp
        public Brush MajorTickBrush
        {
            get { return (Brush)GetValue(MajorTickBrushProperty); }
            set { SetValue(MajorTickBrushProperty, value); }
        }

        public static readonly DependencyProperty MajorTickBrushProperty =
            DependencyProperty.Register(nameof(MajorTickBrush), typeof(Brush), typeof(DirectionControl), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5856d6"))));
        #endregion


        #region StrokeThickness dp
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(DirectionControl), new PropertyMetadata(1.0d));
        #endregion


        #region MajorTickStrokeThickness dp
        public double MajorTickStrokeThickness
        {
            get { return (double)GetValue(MajorTickStrokeThicknessProperty); }
            set { SetValue(MajorTickStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty MajorTickStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MajorTickStrokeThickness), typeof(double), typeof(DirectionControl), new PropertyMetadata(2.0d));
        #endregion


        #region MajorTickSize dp
        public double MajorTickSize
        {
            get { return (double)GetValue(MajorTickSizeProperty); }
            set { SetValue(MajorTickSizeProperty, value); }
        }

        public static readonly DependencyProperty MajorTickSizeProperty =
            DependencyProperty.Register(nameof(MajorTickSize), typeof(double), typeof(DirectionControl), new PropertyMetadata(8.0d));
        #endregion


        #region MinorTickStrokeThickness dp
        public double MinorTickStrokeThickness
        {
            get { return (double)GetValue(MinorTickStrokeThicknessProperty); }
            set { SetValue(MinorTickStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty MinorTickStrokeThicknessProperty =
            DependencyProperty.Register(nameof(MinorTickStrokeThickness), typeof(double), typeof(DirectionControl), new PropertyMetadata(1.0d));
        #endregion


        #region MajorTickSize dp
        public double MinorTickSize
        {
            get { return (double)GetValue(MinorTickSizeProperty); }
            set { SetValue(MinorTickSizeProperty, value); }
        }

        public static readonly DependencyProperty MinorTickSizeProperty =
            DependencyProperty.Register(nameof(MinorTickSize), typeof(double), typeof(DirectionControl), new PropertyMetadata(4.0d));
        #endregion


        #region LabelPadding dp
        public double LabelPadding
        {
            get { return (double)GetValue(LabelPaddingProperty); }
            set { SetValue(LabelPaddingProperty, value); }
        }

        public static readonly DependencyProperty LabelPaddingProperty =
            DependencyProperty.Register(nameof(LabelPadding), typeof(double), typeof(DirectionControl), new PropertyMetadata(25.0d));
        #endregion


        #region Sectors dp
        public IEnumerable<Sector> Sectors
        {
            get { return (IEnumerable<Sector>)GetValue(SectorsProperty); }
            set { SetValue(SectorsProperty, value); }
        }

        public static readonly DependencyProperty SectorsProperty =
            DependencyProperty.Register(nameof(Sectors), typeof(IEnumerable<Sector>), typeof(DirectionControl), new PropertyMetadata(null, propertyChangedCallback: new PropertyChangedCallback(OnSectorsChanged)));

        private static void OnSectorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DirectionControl)d).OnSectorsChanged(e);
        }

        private void OnSectorsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged oldValue)
            {
                oldValue.CollectionChanged -= Sectors_CollectionChanged;
            }

            if (e.NewValue is INotifyCollectionChanged newValue)
            {
                newValue.CollectionChanged += Sectors_CollectionChanged;
            }

            ReDraw();
        }

        private void Sectors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // todo remove|add in grid
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        ReDraw();
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        ReDraw();
                        break;
                    }
            }
        }
        #endregion


        #region Arrows dp
        public IEnumerable<Arrow> Arrows
        {
            get { return (IEnumerable<Arrow>)GetValue(ArrowsProperty); }
            set { SetValue(ArrowsProperty, value); }
        }

        public static readonly DependencyProperty ArrowsProperty =
            DependencyProperty.Register(nameof(Arrows), typeof(IEnumerable<Arrow>), typeof(DirectionControl), new PropertyMetadata(null, propertyChangedCallback: new PropertyChangedCallback(OnArrowsChanged)));

        private static void OnArrowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DirectionControl)d).OnArrowsChanged(e);
        }

        private void OnArrowsChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged oldValue)
            {
                oldValue.CollectionChanged -= Arrows_CollectionChanged;
            }

            if (e.NewValue is INotifyCollectionChanged newValue)
            {
                newValue.CollectionChanged += Arrows_CollectionChanged;
            }

            ReDraw();
        }

        private void Arrows_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // todo remove|add in grid
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        ReDraw();
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        ReDraw();
                        break;
                    }
            }
        }
        #endregion


        #region Constructor
        public DirectionControl()
        {
            InitializeComponent();

            //Loaded += DirectionControl_Loaded;
            ReDraw();
        }

        private Path _sector;
        private void DirectionControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= DirectionControl_Loaded;
            //ReDraw();
            //MouseDown += DirectionControl_MouseDown;
            //MouseUp += DirectionControl_MouseUp;
            //MouseLeave += DirectionControl_MouseLeave;
            SizeChanged += DirectionControl_SizeChanged;
        }

        private void DirectionControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReDraw();
        }

        private void DirectionControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_isMouseDown) return;

            grid.Children.Remove(_sector);
            _isMouseDown = false;
        }

        private void DirectionControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            grid.Children.Remove(_sector);
            _isMouseDown = false;
        }

        private bool _isMouseDown = false;
        private void DirectionControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var pt = e.MouseDevice.GetPosition(this);

            double radius = grid.ActualWidth / 2;

            if (!Intersect(pt, radius))
            {
                return;
            }

            _isMouseDown = true;

            double tan2 = ToDegree(Math.Atan2(pt.X - radius, radius - pt.Y));
            tan2 = tan2 < 0 ? tan2 + 360 : tan2;

            Path path = new()
            {
                Fill = Brushes.Red
            };

            var geometry = CreateArc2(radius, 45, tan2);
            path.Data = geometry;
            grid.Children.Add(path);
            _sector = path;

            Debug.WriteLine($"clicked angle tan2: {tan2}");
        }
        #endregion


        private void ReDraw()
        {
            for (int i = grid.Children.Count - 1; i >= 0; --i)
            {
                if (grid.Children[i] is Ellipse e && e.Name == "outer") continue;

                grid.Children.RemoveAt(i);
            }

            double startRadian = ToRadian(StartAngleInDegree);
            double endRadian = ToRadian(EndAngleInDegree);
            double angle = endRadian - startRadian;
            double arc = angle / (MajorTicksCount - 0);
            double minor_arc = arc / (MinorTickCount + 1);
            double radius = grid.ActualWidth / 2;
            double majorAngle = startRadian;
            double tick = (EndAngleInDegree - StartAngleInDegree) / MajorTicksCount;

            double majorTickStart = radius - MajorTickStrokeThickness;
            double majorTickEnd = radius - MajorTickSize;
            double label_padding = radius - LabelPadding;
            double minorTickStart = radius - MinorTickStrokeThickness;
            double minorTickEnd = radius - MinorTickSize;

            if (Sectors is not null)
            {
                foreach (var sector in Sectors)
                {
                    CreateArc2(radius, sector);
                }
            }

            if (Arrows is not null)
            {
                foreach (var arrow in Arrows)
                {
                    CreateArrow(radius, arrow);
                }
            }

            // major ticks
            for (int i = 0; i < MajorTicksCount; i++)
            {
                Polyline polyline = new()
                {
                    Fill = MajorTickBrush,
                    Stroke = MajorTickBrush,
                    StrokeThickness = MajorTickStrokeThickness,
                    StrokeStartLineCap = PenLineCap.Square,
                    StrokeEndLineCap = PenLineCap.Square
                };
                double cos = Math.Cos(majorAngle);
                double sin = Math.Sin(majorAngle);

                double x = radius + majorTickStart * sin;
                double y = radius - majorTickStart * cos;
                polyline.Points.Add(new Point(x, y));

                x = radius + majorTickEnd * sin;
                y = radius - majorTickEnd * cos;
                polyline.Points.Add(new Point(x, y));
                grid.Children.Add(polyline);

                // minor ticks
                double minorAngle = majorAngle + minor_arc;
                for (int j = 1; j <= MinorTickCount; j++)
                {
                    polyline = new()
                    {
                        Fill = MinorTickBrush,
                        Stroke = MinorTickBrush,
                        StrokeThickness = MinorTickStrokeThickness,
                        StrokeStartLineCap = PenLineCap.Square,
                        StrokeEndLineCap = PenLineCap.Square
                    };

                    double cosMinor = Math.Cos(minorAngle);
                    double sinMinor = Math.Sin(minorAngle);
                    x = radius + minorTickStart * sinMinor;
                    y = radius - minorTickStart * cosMinor;
                    polyline.Points.Add(new Point(x, y));

                    x = radius + minorTickEnd * sinMinor;
                    y = radius - minorTickEnd * cosMinor;
                    polyline.Points.Add(new Point(x, y));
                    grid.Children.Add(polyline);

                    minorAngle += minor_arc;
                }

                // major tick label
                Label label = new();
                string text = (tick * i).ToString();
                label.Content = text;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.Foreground = Foreground;
                label.FontSize = FontSize;

                x = label_padding * sin;
                y = -label_padding * cos;
                label.RenderTransform = new TranslateTransform(x, y);
                grid.Children.Add(label);
                majorAngle += arc;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius">radius of circle</param>
        /// <param name="startAngle">start angle of segment</param>
        /// <param name="endAngle">end angle of segment</param>
        /// <returns>segment</returns>
        private PathGeometry CreateArc2(double radius, double startAngle, double endAngle)
        {
            var diff = endAngle - startAngle;
            Debug.WriteLine($"end angle: {endAngle}, start: {startAngle}, delta: {diff}");
            bool res = diff <= 180 && diff > 0;

            endAngle = ToRadian(endAngle);
            startAngle = ToRadian(startAngle);
            PathGeometry pathGeometry = new();

            PathFigure pathFigure = new();
            pathFigure.IsFilled = true;
            pathFigure.StartPoint = new Point(radius, radius); // take from point
            pathGeometry.Figures.Add(pathFigure);

            LineSegment lineSegment = new();
            lineSegment.Point = new Point(radius, 0); // take from point
            lineSegment.Point = new Point(GetX(radius, startAngle), GetY(radius, startAngle)); // take from point
            pathFigure.Segments.Add(lineSegment);

            ArcSegment arcSegment = new();
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            arcSegment.IsLargeArc = !res;
            arcSegment.RotationAngle = 0;
            arcSegment.Size = new Size(radius, radius);
            arcSegment.Point = new Point(GetX(radius, endAngle), GetY(radius, endAngle));
            pathFigure.Segments.Add(arcSegment);

            return pathGeometry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius">radius of circle</param>
        /// <param name="startAngle">start angle of segment</param>
        /// <param name="endAngle">end angle of segment</param>
        /// <returns>segment</returns>
        private void CreateArc2(double radius, Sector sector)
        {
            var endAngle = sector.EndAngle;
            var startAngle = sector.StartAngle;

            //var endAngle = sector.EndAngle % 360;
            //endAngle = endAngle == 0 ? 360 : endAngle;
            //var startAngle = sector.StartAngle % 360;
            var diff = endAngle - startAngle;
            Debug.WriteLine($"end angle: {sector.EndAngle}, start: {sector.StartAngle}, delta: {diff}");
            bool res = diff <= 180;

            if (diff > 360)
            {
                res = (diff % 360) <= 180.0;
            }

            endAngle = ToRadian(sector.EndAngle);
            startAngle = ToRadian(sector.StartAngle);
            PathGeometry pathGeometry = new();

            PathFigure pathFigure = new();
            pathFigure.IsFilled = true;
            pathFigure.StartPoint = new Point(radius, radius); // take from point
            pathGeometry.Figures.Add(pathFigure);

            LineSegment lineSegment = new();
            lineSegment.Point = new Point(radius, 0); // take from point
            lineSegment.Point = new Point(GetX(radius, startAngle), GetY(radius, startAngle)); // take from point
            pathFigure.Segments.Add(lineSegment);

            ArcSegment arcSegment = new();
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            arcSegment.IsLargeArc = !res;
            arcSegment.RotationAngle = 0;
            arcSegment.Size = new Size(radius, radius);
            arcSegment.Point = new Point(GetX(radius, endAngle), GetY(radius, endAngle));
            pathFigure.Segments.Add(arcSegment);

            SolidColorBrush fill = new BrushConverter().ConvertFromString(sector.Fill) as SolidColorBrush ?? Brushes.Red;
            SolidColorBrush stroke = new BrushConverter().ConvertFromString(sector.Stroke) as SolidColorBrush ?? Brushes.Red;
            fill.Opacity = sector.Opacity;
            stroke.Opacity = sector.Opacity;

            Path path = new()
            {
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = sector.StrokeThickness,
                Data = pathGeometry,
            };

            grid.Children.Add(path);
        }

        private void CreateArrow(double radius, Arrow arrow)
        {
            var startAngle = ToRadian(arrow.Angle);

            PathGeometry pathGeometry = new();
            PathFigure pathFigure = new()
            {
                IsFilled = true,
                StartPoint = new Point(radius, radius),
            };
            pathGeometry.Figures.Add(pathFigure);

            double x1 = radius;
            double y1 = radius;

            double x2 = radius + (radius - arrow.ArrowHeight) * Math.Sin(startAngle);
            double y2 = radius - (radius - arrow.ArrowHeight) * Math.Cos(startAngle);

            double theta = Math.Atan2(y1 - y2, x1 - x2);
            double sin = Math.Sin(theta);
            double cos = Math.Cos(theta);

            var arrowLeft = new Point(x2 + (arrow.ArrowWidth * cos - arrow.ArrowHeight * sin), y2 + (arrow.ArrowWidth * sin + arrow.ArrowHeight * cos));
            var arrowRight = new Point(x2 + (arrow.ArrowWidth * cos + arrow.ArrowHeight * sin), y2 - (arrow.ArrowHeight * cos - arrow.ArrowWidth * sin));

            LineSegment lineSegment = new()
            {
                Point = new Point(x2, y2)
            };
            pathFigure.Segments.Add(lineSegment);


            PathFigure triangle = new()
            {
                IsFilled = true,
                StartPoint = new Point(x2, y2),
            };
            pathGeometry.Figures.Add(triangle);
            triangle.Segments.Add(new LineSegment()
            {
                Point = arrowLeft
            });

            triangle.Segments.Add(new LineSegment()
            {
                Point = new Point(x2, y2),
                IsStroked = false
            });

            triangle.Segments.Add(new LineSegment()
            {
                Point = arrowRight
            });

            triangle.Segments.Add(new LineSegment()
            {
                Point = arrowLeft,
            });

            //PolyLineSegment polyLineSegment = new();
            //polyLineSegment.Points.Add(new Point(x2, y2));
            //polyLineSegment.Points.Add(arrowLeft);
            //polyLineSegment.Points.Add(new Point(x2, y2));
            //pathFigure.Segments.Add(polyLineSegment);


            SolidColorBrush stroke = new BrushConverter().ConvertFromString(arrow.Stroke) as SolidColorBrush ?? Brushes.Red;
            stroke.Opacity = arrow.Opacity;

            Path path = new()
            {
                Stroke = stroke,
                StrokeThickness = arrow.StrokeThickness,
                Fill = stroke,
                Data = pathGeometry
            };
            grid.Children.Add(path);
        }

        private static double ToRadian(double degree)
        {
            return degree * Math.PI / 180.0d;
        }

        private static double ToDegree(double radian)
        {
            return radian * 180 / Math.PI;
        }

        private bool Intersect(Point pt, double radius)
        {
            double d = Math.Sqrt(Math.Pow(pt.X - radius, 2) + Math.Pow(pt.Y - radius, 2));

            return d <= radius;
        }

        private double GetX(double radius, double angle)
        {
            return radius + radius * Math.Sin(angle);
        }

        private double GetY(double radius, double angle)
        {
            return radius - radius * Math.Cos(angle);
        }
    }

    public sealed class Sector
    {
        /// <summary>
        /// Degree
        /// </summary>
        public double StartAngle { get; set; }

        /// <summary>
        /// Degree
        /// </summary>
        public double EndAngle { get; set; }

        public string Fill { get; set; } = "#b4eeb4";

        public string Stroke { get; set; } = "#b4eeb4";

        public double StrokeThickness { get; set; } = 1.0; 

        public double Opacity { get; set; } = 1.0;
    }

    public sealed class Arrow
    {
        public double ArrowWidth { get; set; } = 12;

        public double ArrowHeight { get; set; } = 8;

        public double Angle { get; set; }

        public double Opacity { get; set; } = 1.0;

        public string Stroke { get; set; } = "#008080";

        public double StrokeThickness { get; set; } = 1.0;
    }
}


/*
 
 <Path Fill="Black"
      Data="M0,0 L0,-100 A100,100 0 0 1 70.7,-70.7 z" />

 The Data property uses Path Markup Syntax.

--The "M" at the beginning tells the pen to Move to the location 0,0.
--The "L" tells the pen to draw a Line from the current location (0, 0) to 0,-100.
--The "A" tells the pen to draw an elliptical Arc from the current location to 70.7,-70.7 (the "100,100" portion determines the 
    horizontal and vertical radius of the ellipse and the "0 0 1" portion is for RotationAngle, IsLargeArc, and SweepDirection (1 for clockwise, 0 for counter-clockwise)).
--The "z" tells the pen to close or complete the shape (which will cause a line to be drawn from 70.7,-70.7 back to 0,0).

Where did the 70.7 come from? Well, this particular arc sweeps out an angle of 45 degrees from a circle with radius 100, so the coordinates 70.7,-70.7 are determined by 100 * sin(45) and 100 * cos(45).

 */