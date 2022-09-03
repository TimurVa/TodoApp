using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VlvCustomControlsDotNet
{
    [TemplatePart(Name= "PART_GridContainer", Type=typeof(Grid))]
    [TemplatePart(Name= "PART_TextBlock", Type=typeof(TextBlock))]
    [TemplatePart(Name= "PART_TextBox", Type=typeof(TextBox))]
    public class EditableTextBlock : Control
    {
        #region Private members 

        private Grid _gridContainer;
        private TextBlock _textBlockContainer;
        private TextBox _textBoxContainer;
        private Border _borderContainer;

        #endregion


        #region Name changed routed event
        public static readonly RoutedEvent NameChangedEvent = EventManager.RegisterRoutedEvent("NameChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EditableTextBlock));

        /// <summary>
        /// This event will fire any time TextBox container lost its focus
        /// </summary>
        public event RoutedEventHandler NameChanged
        {
            add { AddHandler(NameChangedEvent, value); }
            remove { RemoveHandler(NameChangedEvent, value); }
        }
        #endregion


        #region Text inside textblock & textbox DP

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(EditableTextBlock), 
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Text of this control
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        #endregion


        #region CaretBrush color for TextBox DP

        public Brush CaretBrush
        {
            get { return (Brush)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }

        public static readonly DependencyProperty CaretBrushProperty =
            DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(EditableTextBlock), new UIPropertyMetadata(Brushes.Transparent));

        #endregion


        #region BorderThickness DP

        public static readonly DependencyProperty VlvBorderThicknessProperty =
            DependencyProperty.Register("VlvBorderThickness", typeof(Thickness), typeof(EditableTextBlock), new UIPropertyMetadata(new Thickness(1)));

        public Thickness VlvBorderThickness
        {
            get { return (Thickness)GetValue(VlvBorderThicknessProperty); }
            set { SetValue(VlvBorderThicknessProperty, value); }
        }

        #endregion


        #region BorderBrush DP

        public static readonly DependencyProperty VlvBorderBrushProperty =
            DependencyProperty.Register("VlvBorderBrush", typeof(Brush), typeof(EditableTextBlock), new UIPropertyMetadata(Brushes.Red));

        public Brush VlvBorderBrush
        {
            get { return (Brush)GetValue(VlvBorderBrushProperty); }
            set { SetValue(VlvBorderBrushProperty, value); }
        }

        #endregion


        #region Text box background color DP

        public Brush TextBoxBackgroundColor
        {
            get { return (Brush)GetValue(TextBoxBackgroundColorProperty); }
            set { SetValue(TextBoxBackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBoxBackgroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxBackgroundColorProperty =
            DependencyProperty.Register("TextBoxBackgroundColor", typeof(Brush), typeof(EditableTextBlock), new UIPropertyMetadata(Brushes.Transparent));

        #endregion


        #region Text box foreground color Dp
        public Brush TextBoxForeground
        {
            get { return (Brush)GetValue(TextBoxForegroundProperty); }
            set { SetValue(TextBoxForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBoxForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxForegroundProperty =
            DependencyProperty.Register("TextBoxForeground", typeof(Brush), typeof(EditableTextBlock), new PropertyMetadata(Brushes.Black));

        #endregion


        #region IsNumericOnly DP
        public bool IsNumericOnly
        {
            get { return (bool)GetValue(IsNumericOnlyProperty); }
            set { SetValue(IsNumericOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNumericOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNumericOnlyProperty =
            DependencyProperty.Register("IsNumericOnly", typeof(bool), typeof(EditableTextBlock), new UIPropertyMetadata(default(bool), OnIsNumericOnlyChanged));

        private static void OnIsNumericOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as EditableTextBlock;

            if (tb != null)
            {
                bool newVal = (bool)e.NewValue;

                if (newVal)
                {
                    tb.PreviewTextInput += Tb_PreviewTextInput;
                    //tb.PreviewKeyDown += Tb_PreviewKeyDown;
                    DataObject.AddPastingHandler(tb, TextBox_Pasting);
                }
                else
                {
                    tb.PreviewTextInput -= Tb_PreviewTextInput;
                    //tb.PreviewKeyDown -= Tb_PreviewKeyDown;
                    DataObject.RemovePastingHandler(tb, TextBox_Pasting);
                }
            }
        }
        #endregion


        #region Max length DP
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(EditableTextBlock), new UIPropertyMetadata(defaultValue: int.MaxValue));
        #endregion


        #region TextBlock textwrapping DP
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(EditableTextBlock), new UIPropertyMetadata(TextWrapping.Wrap));
        #endregion


        #region Text alignment DP
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(EditableTextBlock), new PropertyMetadata(TextAlignment.Left));
        #endregion


        #region Placeholedr text DP
        /// <summary>
        /// Text which would appear if <see cref="TextProperty"/> would be null or empty
        /// </summary>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(EditableTextBlock), new PropertyMetadata(default(string)));
        #endregion


        #region Should handle on mouse down DP

        /// <summary>
        /// If true e.handled would be true on mouse down event on this.
        /// Usefull for ex. in Expander. Standard behaviour is to open expander on mouse click, but 
        /// if this property would be set to true expander will not expand itself.
        /// <para>Default value is false</para>
        /// </summary>
        public bool ShouldHandleOnMouseDown
        {
            get { return (bool)GetValue(ShouldHandleOnMouseDownProperty); }
            set { SetValue(ShouldHandleOnMouseDownProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShouldHangleOnMouseDown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShouldHandleOnMouseDownProperty =
            DependencyProperty.Register("ShouldHandleOnMouseDown", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(false));


        #endregion


        #region Is positive numbers only DP

        /// <summary>
        /// This property would not take any effect if <see cref="IsNumericOnly"/> set to false
        /// </summary>
        public bool IsPositiveNumbersOnly
        {
            get { return (bool)GetValue(IsPositiveNumbersOnlyProperty); }
            set { SetValue(IsPositiveNumbersOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsPositiveNumbersOnlyProperty =
            DependencyProperty.Register("IsPositiveNumbersOnly", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(false));
        #endregion



        static EditableTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableTextBlock), new FrameworkPropertyMetadata(typeof(EditableTextBlock)));

            IsEnabledProperty.OverrideMetadata(typeof(EditableTextBlock),
                    new System.Windows.UIPropertyMetadata(true,
                    new PropertyChangedCallback(IsEnabledPropertyChanged),
                    new CoerceValueCallback(CoerceIsEnabled)));
        }

        private static object CoerceIsEnabled(DependencyObject source, object value)
        {
            return value;
        }

        private static void IsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            // Overriding PropertyChanged results in merged metadata, which is what we want--
            // the PropertyChanged logic in UIElement.IsEnabled will still get invoked.
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _gridContainer = Template.FindName("PART_GridContainer", this) as Grid;

            if (_gridContainer != null)
            {
                _textBlockContainer = Template.FindName("PART_TextBlock", this) as TextBlock;
                _textBoxContainer = Template.FindName("PART_TextBox", this) as TextBox;
                _borderContainer = Template.FindName("PART_Border", this) as Border;

                LostFocus += EditableTextBlock_LostFocus;

                _textBoxContainer.LostFocus += OnLostFocus;
                _textBoxContainer.KeyUp += OnKeyUp;
                _textBoxContainer.GotFocus += _textBoxContainer_GotFocus;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (ShouldHandleOnMouseDown)
            {
                e.Handled = true;
            }
        }

        private void EditableTextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            RaiseNameChangedEvent();
        }

        private void _textBoxContainer_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;

            tb.SelectAll();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                //Keyboard.ClearFocus();
                //_textBlockContainer.Focus();
                //_textBoxContainer.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                if (e.Key == Key.Escape)
                {
                    var thisBinding = GetBindingExpression(TextProperty);

                    var binding = _textBoxContainer.GetBindingExpression(TextBox.TextProperty);

                    if (binding != null && thisBinding != null && thisBinding.ParentBinding != null && thisBinding.ParentBinding is System.Windows.Data.Binding b)
                    {
                        if (b.UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.LostFocus)
                        {
                            binding.UpdateTarget();
                        }
                    }
                }

                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(_textBoxContainer), null);
                Keyboard.ClearFocus();

                ChangeVisibility();
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            ChangeVisibility();

        }

        #region TB Preview key down event
        private static void Tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text.Length == 1 && (e.Key == Key.Delete || e.Key == Key.Back))
            {
                tb.Text = "0";
                tb.CaretIndex = 1;
                e.Handled = true;
            }
            else if (tb.Text == "0")
                tb.Clear();
            else
                e.Handled = e.Key == Key.Space;

        }
        #endregion


        #region TB preview text input event
        private static void Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            EditableTextBlock tb = (EditableTextBlock)sender;
            string txt = GetFullText(tb._textBoxContainer, e.Text);

            if (tb.IsPositiveNumbersOnly)
            {
                e.Handled = !IsOnlyPositiveNumbers(txt);
            }
            else
            {
                e.Handled = !IsTextValid(txt);
            }
        }
        #endregion


        #region Pasting event
        private static void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                EditableTextBlock etb = (EditableTextBlock)sender;
                string text = GetFullText(etb._textBoxContainer, (string)e.DataObject.GetData(typeof(string)));

                if (etb.IsPositiveNumbersOnly)
                {
                    if (!IsOnlyPositiveNumbers(text))
                    {
                        e.CancelCommand();
                    }
                }
                else
                {
                    if (!IsTextValid(text))
                    {
                        e.CancelCommand();
                    }
                }
            }
            else
                e.CancelCommand();
        }
        #endregion


        //protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        //{
        //    base.OnMouseDoubleClick(e);

        //    _textBlockContainer.Visibility = Visibility.Hidden;
        //    _borderContainer.Visibility = Visibility.Visible;
        //    _textBoxContainer.Focus();
        //}

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
            _textBlockContainer.Visibility = Visibility.Hidden;
            //_borderContainer.Visibility = Visibility.Visible;
            _textBoxContainer.Visibility = Visibility.Visible;
            _textBoxContainer.Focus();

            e.Handled = true;
        }


        #region Helpers 

        private void ChangeVisibility()
        {
            _textBlockContainer.Visibility = Visibility.Visible;
            //_borderContainer.Visibility = Visibility.Hidden;
            _textBoxContainer.Visibility = Visibility.Hidden;
        }

        private void RaiseNameChangedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(NameChangedEvent);
            RaiseEvent(args);
        }

        /// <summary>
        /// Returns text between selection or just string with inserted text from textbox
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetFullText(TextBox tb, string text)
        {
            return tb.SelectedText.Length > 0 ? string.Concat(tb.Text.Substring(0, tb.SelectionStart), text, tb.Text.Substring(tb.SelectionStart + tb.SelectedText.Length)) : tb.Text.Insert(tb.SelectionStart, text);
        }

        private static readonly Regex _numbersRegex = new Regex(@"^\d\.?\d*$");
        /// <summary>
        /// Regex determines if given string is has valid numeric format
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool IsTextValid(string text)
        {
            return _numbersRegex.Match(text).Success;
            //return Regex.Match(text, @"^\d\.?\d*$").Success;
        }

        private static bool IsOnlyPositiveNumbers(string text)
        {
            bool result = double.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double val);

            if (result == false)
            {
                return false;
            }

            return val > 0;
        }
        #endregion
    }
}
