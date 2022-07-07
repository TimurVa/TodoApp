using System.Windows;
using ToDoApp.ViewModels;

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Ioc.IocContainer.Setup();

            MainWindow window = new MainWindow();
            MainViewModel mainViewModel = new MainViewModel();
            window.DataContext = mainViewModel;

            window.Show();
        }
    }
}
