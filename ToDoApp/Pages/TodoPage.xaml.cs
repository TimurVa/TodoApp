using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToDoApp.Pages
{
    /// <summary>
    /// Interaction logic for TodoPage.xaml
    /// </summary>
    public partial class TodoPage : Page
    {
        public TodoPage()
        {
            InitializeComponent();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //FiltersRow.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //FiltersRow.Visibility = Visibility.Collapsed;
        }
    }
}
