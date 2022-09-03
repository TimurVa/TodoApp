using System;
using System.Windows;
using System.Windows.Media;

namespace VlvCustomControlsDotNet
{
    /// <summary>
    /// Interaction logic for VlvCustomMessageBox.xaml
    /// </summary>
    public partial class VlvCustomMessageBoxWindow : Window
    {
#pragma warning disable 0169
        private readonly MessageBoxButton btn;

        #region Properties

        public MessageBoxResult Result { get; private set; } = MessageBoxResult.Cancel;

        internal string Message
        {
            get
            {
                return MessagePlaceholder.Text;
            }
            set
            {
                MessagePlaceholder.Text = value;
            }
        }

        public string WindowTitle { get; set; }

        #endregion

        //#region Image Source DP

        //public ImageSource ImageSource
        //{
        //    get { return (ImageSource)GetValue(ImageSourceProperty); }
        //    set { SetValue(ImageSourceProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ImageSourceProperty =
        //    DependencyProperty.Register("MyProperty", typeof(ImageSource), typeof(VlvCustomMessageBox), new PropertyMetadata(0));

        //#endregion

        #region Constructor

        public VlvCustomMessageBoxWindow(string message, string title = "")
        {
            InitializeComponent();
            Message = message;
            Title = title;
        }

        public VlvCustomMessageBoxWindow(string message, MessageBoxButton btn, string title = "") : this(message)
        {
            ShowButton(btn);
            Title = title;
        }

        private void ShowButton(MessageBoxButton btn)
        {
            switch (btn)
            {
                case MessageBoxButton.OKCancel:
                {
                    CancelButton.Visibility = Visibility.Visible;
                    break;
                }
                default:
                    break;
            }
        }

        #endregion

        #region Buttons clicks

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        #endregion

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
