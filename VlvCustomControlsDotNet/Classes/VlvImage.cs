using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VlvCustomControlsDotNet
{
    public class VlvImage : Control
    {
        private Image _image;
        private MediaElement _mediaElement;


        public byte[] ImageSourceArray
        {
            get { return (byte[])GetValue(ImageSourceArrayProperty); }
            set { SetValue(ImageSourceArrayProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceArrayProperty =
            DependencyProperty.Register("ImageSourceArray", typeof(byte[]), typeof(VlvImage), new PropertyMetadata(default(Array), OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VlvImage vlvImage = d as VlvImage;
            vlvImage.UpdateImageSource();
        }

        public string GifUri
        {
            get { return (string)GetValue(GifUriProperty); }
            set { SetValue(GifUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GifUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GifUriProperty =
            DependencyProperty.Register("GifUri", typeof(string), typeof(VlvImage), new PropertyMetadata(null, OnUriChanged));

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VlvImage vlvImage = d as VlvImage;
            vlvImage.UpdateMediaElementSource(e.OldValue, e.NewValue);
        }

        static VlvImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlvImage), new FrameworkPropertyMetadata(typeof(VlvImage)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = Template.FindName("PART_Image", this) as Image;
            _mediaElement = Template.FindName("PART_MediaElement", this) as MediaElement;

            _mediaElement.LoadedBehavior = MediaState.Play;
            _mediaElement.UnloadedBehavior = MediaState.Stop;
        }

        public void UpdateImageSource()
        {
            if (ImageSourceArray == null)
            {
                _image.Visibility = Visibility.Collapsed;
                _image.Source = null;
                return;
            }

            if (_image.Source != null)
            {
                _image.Source = null;
            }

            if (_image.Visibility == Visibility.Collapsed)
            {
                _image.Visibility = Visibility.Visible;
            }

            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
            bitmap.StreamSource = new MemoryStream(ImageSourceArray);
            bitmap.EndInit();
            bitmap.Freeze();

            _image.Source = bitmap;
        }

        public void UpdateMediaElementSource(object oldVal, object newVal)
        {
            if (oldVal == null && newVal != null)
            {
                _mediaElement.MediaEnded += _mediaElement_MediaEnded;
            }

            if (oldVal != null && newVal == null)
            {
                _mediaElement.Visibility = Visibility.Collapsed;
                _mediaElement.MediaEnded -= _mediaElement_MediaEnded;
                _mediaElement.Source = null;
            }

            if (!File.Exists(GifUri))
            {
                return;
            }

            if (_mediaElement.Visibility == Visibility.Collapsed)
            {
                _mediaElement.Visibility = Visibility.Visible;
            }

            Uri uri = new Uri(GifUri);

            _mediaElement.Source = uri;
        }

        private void _mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            _mediaElement.Position = new TimeSpan(0, 0, 1);
        }
    }
}
