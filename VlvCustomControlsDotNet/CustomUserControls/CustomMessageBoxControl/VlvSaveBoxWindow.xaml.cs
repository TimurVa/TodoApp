using System.Windows;

namespace VlvCustomControlsDotNet
{
    public enum DialogResult : int
    {
        Save = 0,
        Donot = 1,
        Cancel = 2
    }
    /// <summary>
    /// Interaction logic for VlvSaveBoxWindow.xaml
    /// </summary>
    public partial class VlvSaveBoxWindow : Window
    {
        public DialogResult DialogResult { get; private set; } = DialogResult.Cancel;
            
        public string Message
        {
            get
            {
                return MessagePlaceholder.Text;
            }
            private set
            {
                MessagePlaceholder.Text = value;
            }
        }

        public string WindowTitle { get; set; }

        public VlvSaveBoxWindow(string message, string title = "")
        {
            InitializeComponent();
            Message = message;
            Title = title;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Dont_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Donot;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Save;
            Close();
        }
    }
}
