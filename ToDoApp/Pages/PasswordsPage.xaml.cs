using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ToDoApp.Pages
{
    /// <summary>
    /// Interaction logic for PasswordsPage.xaml
    /// </summary>
    public partial class PasswordsPage : Page
    {
        public PasswordsPage()
        {
            InitializeComponent();
        }

        private void Popup_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((Popup)sender).IsOpen = false;
        }
    }
}
