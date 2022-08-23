using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToDoApp.Helpers
{
    public class VlvListBoxScrollEnded : ListBox
    {
        public ScrollViewer ScrollViewer { get; private set; }

        public VlvListBoxScrollEnded()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ScrollViewer = Static.FindDescendant<ScrollViewer>(this);

            if (ScrollViewer != null)
            {
                ScrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        object locker = new();
        private double _prevOffset = 0;
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            lock (locker)
            {
                //System.Diagnostics.Debug.WriteLine("_prevOffset" + _prevOffset);
                //System.Diagnostics.Debug.WriteLine("ScrollViewer.VerticalOffset " + ScrollViewer.VerticalOffset);
                //System.Diagnostics.Debug.WriteLine("ScrollViewer.ScrollableHeight " + ScrollViewer.ScrollableHeight);
                //System.Diagnostics.Debug.WriteLine("ScrollViewer.Height " + ScrollViewer.Height);
                //System.Diagnostics.Debug.WriteLine("ScrollViewer.ExtentHeight " + ScrollViewer.ExtentHeight);
                //System.Diagnostics.Debug.WriteLine("Listbox.Height " + ActualHeight);

                if (!double.IsNaN(ActualHeight) && ScrollViewer.ExtentHeight < ActualHeight)
                {
                    //System.Diagnostics.Debug.WriteLine("ScrollViewer.ExtentHeight too small");

                    var command = ScrollEndedCommand;

                    if (command == null || !command.CanExecute(null))
                        return;

                    command.Execute(null);
                    _prevOffset = ScrollViewer.VerticalOffset;
                }

                if (_prevOffset == ScrollViewer.VerticalOffset)
                {
                    //System.Diagnostics.Debug.WriteLine("Same offset");
                    return;
                }

                if (ScrollViewer.VerticalOffset >= ScrollViewer.ScrollableHeight)
                {
                    //System.Diagnostics.Debug.WriteLine("Scrolling to end");

                    var command = ScrollEndedCommand;

                    if (command == null || !command.CanExecute(null))
                        return;

                    command.Execute(null);
                    _prevOffset = ScrollViewer.VerticalOffset;
                }
            }
        }

        //public ICommand ScrollEndedCommand
        //{
        //    get { return (ICommand)GetValue(ScrollEndedCommandProperty); }
        //    set { SetValue(ScrollEndedCommandProperty, value); }
        //}

        //public static readonly DependencyProperty ScrollEndedCommandProperty =
        //    DependencyProperty.Register("ScrollEndedCommand",
        //        typeof(ICommand),
        //        typeof(VlvListBoxScrollEnded),
        //        new PropertyMetadata(default(ICommand), OnCommandChanged));

        public ICommand ScrollEndedCommand
        {
            get { return (ICommand)GetValue(ScrollEndedCommandProperty); }
            set { SetValue(ScrollEndedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollEndedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollEndedCommandProperty =
            DependencyProperty.Register("ScrollEndedCommand", typeof(ICommand), typeof(VlvListBoxScrollEnded), new PropertyMetadata(default(ICommand)));

        //private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var lb = d as VlvListBoxScrollEnded;

        //    if (e.OldValue == null && e.NewValue != null)
        //    {
        //        lb.Loaded += FrameworkElement_Loaded;
        //    }
        //    else if (e.OldValue != null && e.NewValue == null)
        //    {
        //        lb.Loaded -= FrameworkElement_Loaded;
        //    }

        //}

        //private static void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var lb = sender as VlvListBoxScrollEnded;

        //    if (lb == null || lb.ScrollViewer == null)
        //    {
        //        return;
        //    }

        //    lb.Loaded -= FrameworkElement_Loaded;
        //    lb.Unloaded += OnFrameworkElementUnloaded;
        //    lb.ScrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
        //}

        //private static void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        //{
        //    var scrollViewer = sender as ScrollViewer;

        //    if (scrollViewer == null)
        //    {
        //        return;
        //    }

        //    if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
        //    {
        //        var command = ScrollEndedCommand(null);
        //        if (command == null || !command.CanExecute(null))
        //            return;

        //        command.Execute(null);
        //    }
        //}

        //private static void OnFrameworkElementUnloaded(object sender, RoutedEventArgs e)
        //{
        //    var lb = sender as VlvListBoxScrollEnded;

        //    if (lb == null || lb.ScrollViewer == null)
        //    {
        //        return;
        //    }

        //    lb.Unloaded -= OnFrameworkElementUnloaded;
        //    lb.ScrollViewer.ScrollChanged -= OnScrollViewerScrollChanged;
        //}


    }
}
