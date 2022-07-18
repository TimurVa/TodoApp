using System.Windows;
using System.Windows.Input;

namespace ToDoApp.Helpers.Attached
{
    public class FrameworkElementAttachedProperties
    {
        public static ICommand GetLoadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(LoadedCommandProperty);
        }

        public static void SetLoadedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(LoadedCommandProperty, value);
        }

        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.RegisterAttached("LoadedCommand", typeof(ICommand), typeof(FrameworkElementAttachedProperties), new PropertyMetadata(null, OnLoaded));

        private static void OnLoaded(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            if (e.OldValue == null && e.NewValue != null)
            {
                frameworkElement.Loaded += FrameworkElement_Loaded;
            }
            else if (e.OldValue != null && e.NewValue == null)
            {
                frameworkElement.Loaded -= FrameworkElement_Loaded;
            }
        }

        private static void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;

            ICommand cmd = GetLoadedCommand(frameworkElement);

            if (cmd != null && cmd.CanExecute(null))
            {
                cmd.Execute(null);
            }
        }
    }
}
