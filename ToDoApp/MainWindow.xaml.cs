using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Grid _menu;
        public MainWindow()
        {
            InitializeComponent();


            //Loaded += MainWindow_Loaded;
            //StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Margin = new Thickness(5);

                if (_menu != null)
                {
                    _menu.Margin = new Thickness(0, 3, 7, 0);
                }
            }
            else
            {
                if (_menu != null)
                {
                    _menu.Margin = new Thickness(0);
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _menu = Template.FindName("Menu", this) as Grid;
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
