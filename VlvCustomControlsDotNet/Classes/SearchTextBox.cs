using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VlvCustomControlsDotNet
{
    public class SearchTextBox : Control
    {
        private TextBox _textBox;

        #region Text property DP
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(SearchTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTextChanged))
                {
                    DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
                });

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion


        #region Button style dp
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(SearchTextBox), new PropertyMetadata(new Style(typeof(Button))));
        #endregion


        #region Search command DP
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchTextBox), new PropertyMetadata(default(ICommand)));
        #endregion


        #region Placeholder text DP
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(SearchTextBox), new PropertyMetadata(string.Empty));
        #endregion


        #region Corner radius DP
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SearchTextBox), new PropertyMetadata(default(CornerRadius)));
        #endregion


        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTextBox), new FrameworkPropertyMetadata(typeof(SearchTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBox = Template.FindName("PART_Textbox", this) as TextBox;

            if (_textBox != null)
            {
                _textBox.KeyUp += _textBox_KeyUp;

                var binding = GetBindingExpression(TextProperty)?.ParentBinding;

                _textBox.SetBinding(TextBox.TextProperty, binding);
            }
        }

        private void _textBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                if (e.Key == Key.Escape)
                {
                    var thisBinding = GetBindingExpression(TextProperty);

                    var binding = _textBox.GetBindingExpression(TextBox.TextProperty);

                    if (thisBinding != null && thisBinding.ParentBinding != null && thisBinding.ParentBinding is System.Windows.Data.Binding b)
                    {
                        if (b.UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.LostFocus)
                        {
                            binding.UpdateTarget();
                        }
                    }
                }

                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(_textBox), null);
                Keyboard.ClearFocus();
            }

        }
    }
}
