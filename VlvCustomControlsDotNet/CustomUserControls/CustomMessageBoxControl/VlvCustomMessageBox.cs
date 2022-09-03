using System.Windows;

namespace VlvCustomControlsDotNet
{
    public static class VlvCustomMessageBox
    {
        public static MessageBoxResult Show(string message, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterOwner, Window owner = null, string title = "")
        {
            VlvCustomMessageBoxWindow vlv = new VlvCustomMessageBoxWindow(message, title);

            if (owner != null)
            {
                vlv.Owner = owner;
            }

            vlv.WindowStartupLocation = windowStartupLocation;
            vlv.ShowDialog();

            return vlv.Result;
        }

        public static MessageBoxResult Show(string message, MessageBoxButton btn, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterOwner, Window owner = null, string title = "")
        {
            VlvCustomMessageBoxWindow vlv = new VlvCustomMessageBoxWindow(message, btn, title);

            if (owner != null)
            {
                vlv.Owner = owner;
            }

            vlv.WindowStartupLocation = windowStartupLocation;

            vlv.ShowDialog();

            return vlv.Result;
        }

        public static DialogResult ShowSaveDialog(string message, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterOwner, Window owner = null, string title = "")
        {
            VlvSaveBoxWindow vlv = new VlvSaveBoxWindow(message, title);

            if (owner != null)
            {
                vlv.Owner = owner;
            }

            vlv.WindowStartupLocation = windowStartupLocation;
            vlv.ShowDialog();

            return vlv.DialogResult;
        }
    }
}
