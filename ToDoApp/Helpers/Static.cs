using System.Threading;

namespace ToDoApp.Helpers
{
    public static class Static
    {
        public static SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1, 1);

        public const byte REPEAT_COUNT = 1;

        public const string LIGHT_THEME_PATH = "Resources/Themes/LightTheme.xaml";
        public const string DARK_THEME_PATH = "Resources/Themes/DarkTheme.xaml";
    }
}
