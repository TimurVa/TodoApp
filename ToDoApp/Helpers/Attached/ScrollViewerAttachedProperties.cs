using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToDoApp.Helpers.Attached
{
    public class ScrollViewerAttachedProperties
    {
        public static ICommand GetScrollEndedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ScrollEndedCommandProperty);
        }

        public static void SetScrollEndedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ScrollEndedCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for ScrollEndedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollEndedCommandProperty =
            DependencyProperty.RegisterAttached("ScrollEndedCommand", 
                typeof(ICommand), 
                typeof(ScrollViewerAttachedProperties), 
                new PropertyMetadata(default(ICommand), OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var scrollViewer = d as ScrollViewer;

            if (e.OldValue == null && e.NewValue != null)
            {
                scrollViewer.Loaded += FrameworkElement_Loaded;
            }
            else if (e.OldValue != null && e.NewValue == null)
            {
                scrollViewer.Loaded -= FrameworkElement_Loaded;
            }

        }

        private static void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            if (scrollViewer == null)
            {
                return;
            }

            scrollViewer.Loaded -= FrameworkElement_Loaded;
            scrollViewer.Unloaded += OnScrollViewerUnloaded;
            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
        }

        private static void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            if (scrollViewer == null)
            {
                return;
            }

            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
            {
                var command = GetScrollEndedCommand(null);
                if (command == null || !command.CanExecute(null))
                    return;

                command.Execute(null);
            }
        }

        private static void OnScrollViewerUnloaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            if (scrollViewer == null)
            {
                return;
            }

            scrollViewer.Unloaded -= OnScrollViewerUnloaded;
            scrollViewer.ScrollChanged -= OnScrollViewerScrollChanged;
        }
    }
}
