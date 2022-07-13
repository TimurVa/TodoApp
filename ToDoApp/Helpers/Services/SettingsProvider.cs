using System.Windows;
using ToDoApp.Interfaces;
using ToDoApp.Pages.Windowed;

namespace ToDoApp.Helpers.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly ISettings _settings;

        public SettingsProvider(ISettings settings)
        {
            _settings = settings;
        }

        public void Show()
        {
            foreach (var wnd in Application.Current.Windows)
            {
                if (wnd is SettingsWindow sw)
                {
                    sw.Activate();
                    return;
                }
            }

            SettingsWindow window = new SettingsWindow();
            window.DataContext = _settings;

            window.Show();
        }
    }
}
