using System.Windows;
using System.Windows.Controls;

namespace VlvCustomControlsDotNet
{
    /// <summary>
    /// Interaction logic for TextBoxControl.xaml
    /// </summary>
    public partial class VlvTextBoxWithPlaceholderControl : UserControl
    {
        public VlvTextBoxWithPlaceholderControl()
        {
            InitializeComponent();
        }

        #region Placeholder text DP

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(VlvTextBoxWithPlaceholderControl));


        #endregion

        #region Text property for TextBox DP

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(VlvTextBoxWithPlaceholderControl), new FrameworkPropertyMetadata(true)
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged,
            });

        #endregion

        #region Boolean for check if this is passwordbox or normal DP

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public static readonly DependencyProperty IsPasswordProperty =
            DependencyProperty.Register("IsPassword", typeof(bool), typeof(VlvTextBoxWithPlaceholderControl));

        #endregion

        #region PasswordBox text changed

        /// <summary>
        /// idk actually is it safe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ActualData.Text = PasswordBox.Password;
        }

        #endregion
    }
}
