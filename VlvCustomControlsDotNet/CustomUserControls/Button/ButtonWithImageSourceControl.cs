using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VlvCustomControlsDotNet
{
    public class ButtonWithImageSourceControl : Control
    {
        #region Image source DP
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ButtonWithImageSourceControl), new PropertyMetadata(null, OnImageSourceChanged));

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion

        #region Image width DP
        public double ImageSourceWidth
        {
            get { return (double)GetValue(ImageSourceWidthProperty); }
            set { SetValue(ImageSourceWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceWidthProperty =
            DependencyProperty.Register("ImageSourceWidth", typeof(double), typeof(ButtonWithImageSourceControl), new PropertyMetadata(7d));
        #endregion

        #region Image heigth DP
        public double ImageSourceHeight
        {
            get { return (double)GetValue(ImageSourceHeightProperty); }
            set { SetValue(ImageSourceHeightProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceHeightProperty =
            DependencyProperty.Register("ImageSourceHeight", typeof(double), typeof(ButtonWithImageSourceControl), new PropertyMetadata(7d));
        #endregion

        #region Margin for Image DP
        public Thickness MarginBetweenImageSource
        {
            get { return (Thickness)GetValue(MarginBetweenImageSourceProperty); }
            set { SetValue(MarginBetweenImageSourceProperty, value); }
        }

        public static readonly DependencyProperty MarginBetweenImageSourceProperty =
            DependencyProperty.Register("MarginBetweenImageSource", typeof(Thickness), typeof(ButtonWithImageSourceControl), new PropertyMetadata(default(Thickness)));
        #endregion

        #region Optional Text DP
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ButtonWithImageSourceControl), new PropertyMetadata(string.Empty));
        #endregion

        static ButtonWithImageSourceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonWithImageSourceControl), new FrameworkPropertyMetadata(typeof(ButtonWithImageSourceControl)));
        }
    }
}
