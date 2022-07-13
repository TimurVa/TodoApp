using System.Windows;
using ToDoApp.Helpers;
using ToDoApp.ViewModels;

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel _mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Ioc.IocContainer.Setup();

            MainWindow window = new MainWindow();
            MainViewModel mainViewModel = new MainViewModel(Ioc.IocContainer.Settings);
            window.DataContext = mainViewModel;

            WindowResizer windowResizer = new WindowResizer(window);

            _mainViewModel = mainViewModel;

            window.Closing += Window_Closing;

            mainViewModel.Completed += (s, e) =>
            {
                window?.Close();
            };

            _mainViewModel.Maximize += (s, e) =>
            {
                if (window == null)
                {
                    return;
                }

                if (window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                    return;
                }

                window.WindowState = WindowState.Maximized;
            };

            _mainViewModel.Minimize += (s, e) =>
            {
                if (window == null)
                {
                    return;
                }

                window.WindowState = WindowState.Minimized;
            };

            window.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
