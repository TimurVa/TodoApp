using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ToDoApp.Controls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBoxControl.xaml
    /// </summary>
    public partial class BindablePasswordBoxControl : UserControl
    {
        private bool _isPasswordChanging;

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBoxControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBoxControl passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public BindablePasswordBoxControl()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                return;
            }

            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }

        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                passwordBox.Password = Password;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passwordUnsecured.Visibility == Visibility.Visible)
            {
                passwordUnsecured.Visibility = Visibility.Hidden;
                passwordBox.Visibility = Visibility.Visible;
            }
            else
            {
                passwordUnsecured.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Hidden;
            }
        }
    }
}
