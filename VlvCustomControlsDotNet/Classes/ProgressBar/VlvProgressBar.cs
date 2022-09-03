using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TVCustomControlLibrary.Enums;

namespace TVCustomControlLibrary
{
    [TemplatePart(Name = "Shape", Type = typeof(ProgressBarShape))]
    [TemplatePart(Name = "PART_Track", Type = typeof(Border))]
    [TemplatePart(Name = "PART_Indicator", Type = typeof(Canvas))]
    public class VlvProgressBar : ProgressBar
    {
        #region Private members

        private ProgressBarShape _shape;
        private Border _border;
        private Canvas _canvas;

        #endregion

        #region Orientation. DP

        public static DependencyProperty OrientationChangedProperty = 
            DependencyProperty.Register(nameof(OrientationChanged), typeof(Orientation), typeof(VlvProgressBar), new PropertyMetadata(Orientation.Vertical, new PropertyChangedCallback(UpdateOrientation)));

        /// <summary>
        /// Gets or sets value of Orientation. Default value is Vertical
        /// </summary>
        public Orientation OrientationChanged
        {
            get => (Orientation)GetValue(OrientationChangedProperty);
            set => SetValue(OrientationChangedProperty, value);
        }

        private static void UpdateOrientation(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VlvProgressBar)d).SwitchAngles();
        }

        #endregion

        #region Type of shape. DP

        public static DependencyProperty ShapeTypeProperty =
            DependencyProperty.Register(nameof(ShapeType), typeof(ShapeType), typeof(VlvProgressBar), 
                new FrameworkPropertyMetadata(new PropertyChangedCallback(UpdateShape)));

        /// <summary>
        /// Gets or sets value of ProgressBar control. Default value is Rectangle
        /// </summary>
        public ShapeType ShapeType
        {
            get => (ShapeType)GetValue(ShapeTypeProperty);
            set => SetValue(ShapeTypeProperty, value);
        }

        #endregion

        #region Text block visibility. DP
        public Visibility TextBlockVisibility
        {
            get { return (Visibility)GetValue(TextBlockVisibilityProperty); }
            set { SetValue(TextBlockVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TextBlockVisibilityProperty =
        // Using a DependencyProperty as the backing store for TextBlockVisibility.  This enables animation, styling, binding, etc...
            DependencyProperty.Register("TextBlockVisibility", typeof(Visibility), typeof(VlvProgressBar), new PropertyMetadata(Visibility.Collapsed));
        #endregion

        #region Text block font size. DP
        public int TextBlockFontSize
        {
            get { return (int)GetValue(TextBlockFontSizeProperty); }
            set { SetValue(TextBlockFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBlockFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBlockFontSizeProperty =
            DependencyProperty.Register("TextBlockFontSize", typeof(int), typeof(VlvProgressBar), new PropertyMetadata(9));
        #endregion

        #region FG color for textblock. DP
        public Brush TextBlockForegroundColorBrush
        {
            get { return (Brush)GetValue(TextBlockForegroundColorBrushProperty); }
            set { SetValue(TextBlockForegroundColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBlockForegroundColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBlockForegroundColorBrushProperty =
            DependencyProperty.Register("TextBlockForegroundColorBrush", typeof(Brush), typeof(VlvProgressBar), new PropertyMetadata(Brushes.Red));
        #endregion

        static VlvProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlvProgressBar), new FrameworkPropertyMetadata(typeof(VlvProgressBar)));
        }

        public VlvProgressBar()
        {
            SizeChanged += VlvProgressBar_SizeChanged;
            Loaded += VlvProgressBar_Loaded;
        }

        private void VlvProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            CheckShape();
        }

        private void VlvProgressBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _shape?.RecalculateBounds(Width, Height);
        }

        public delegate void InitializeComponent();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = (Border)Template.FindName("PART_Track", this);
            _canvas = (Canvas)Template.FindName("PART_Indicator", this);
            _shape = (ProgressBarShape)Template.FindName("Shape", this);

            if (_shape != null && _border != null)
            {
                CheckShape();
                SwitchAngles();
            }
        }

        //private void Initialize()
        //{
        //    if (_border != null)
        //        return;

        //    _border = GetTemplateChild("PART_Track") as Border;
        //    _canvas = GetTemplateChild("PART_Indicator") as Canvas;
        //    _shape = GetTemplateChild("Shape") as ProgressBarShape;
        //}

        #region Helpers
        private void SwitchAngles()
        {
            if (OrientationChanged == Orientation.Vertical)
            {
                if (_shape != null)
                {
                    _shape.LayoutTransform = new RotateTransform()
                    {
                        Angle = -270,
                    };
                    _border.LayoutTransform = new RotateTransform()
                    {
                        Angle = 270,
                    };
                }
            }
            else
            {
                if (_shape != null)
                {
                    _shape.LayoutTransform = new RotateTransform()
                    {
                        Angle = 0,
                    };
                    _border.LayoutTransform = new RotateTransform()
                    {
                        Angle = 0,
                    };
                }
            }
        }

        private static void UpdateShape(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VlvProgressBar pg = d as VlvProgressBar;

            if (pg != null)
                pg.CheckShape();
        }

        private void CheckShape()
        {
            if (_border == null)
                return;

            if (ShapeType == ShapeType.Rectangle)
            {
                _border.CornerRadius = new CornerRadius(0);
            }
            else
            {
                _border.CornerRadius = new CornerRadius(60);
            }
        }
        #endregion

    }
}
