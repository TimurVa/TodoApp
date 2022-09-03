using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace VlvCustomControlsDotNet
{
    [TemplatePart(Name = PART_ColorItems, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_AllColors, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_ToggleButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
    public class VlvColorPicker : Control
    {
        private const string PART_ColorItems = "PART_ColorItems";
        private const string PART_AllColors = "PART_AllColors";
        private const string PART_ToggleButton = "PART_ToggleButton";
        public const string PART_Popup = "PART_Popup";

        private ListBox _colorItems;
        private ListBox _allColorItems;
        private ToggleButton _toggle;
        private Popup _popup;

        #region Selected color DP
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(VlvColorPicker), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #region Color items DP
        public ObservableCollection<ColorItem> ColorItems
        {
            get { return (ObservableCollection<ColorItem>)GetValue(ColorItemsProperty); }
            set { SetValue(ColorItemsProperty, value); }
        }

        public static readonly DependencyProperty ColorItemsProperty =
            DependencyProperty.Register("ColorItems", typeof(ObservableCollection<ColorItem>), typeof(VlvColorPicker), new UIPropertyMetadata(CreateStandardColors()));
        #endregion

        #region All colors DP. NOT USED
        public ObservableCollection<ColorItem> AllColors
        {
            get { return (ObservableCollection<ColorItem>)GetValue(AllColorsProperty); }
            set { SetValue(AllColorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllColors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllColorsProperty =
            DependencyProperty.Register("AllColors", typeof(ObservableCollection<ColorItem>), typeof(VlvColorPicker), new UIPropertyMetadata(CreateAllColors()));
        #endregion

        #region Max drop down width for popup DP
        public double MaxDropDownWidth
        {
            get { return (double)GetValue(MaxDropDownWidthProperty); }
            set { SetValue(MaxDropDownWidthProperty, value); }
        }

        public static readonly DependencyProperty MaxDropDownWidthProperty =
            DependencyProperty.Register("MaxDropDownWidth", typeof(double), typeof(VlvColorPicker), new PropertyMetadata(100d));

        #endregion

        #region Arrow symbol visibility DP
        public bool IsDropDownArrowVisible
        {
            get { return (bool)GetValue(IsDropDownArrowVisibleProperty); }
            set { SetValue(IsDropDownArrowVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownArrowVisibleProperty =
            DependencyProperty.Register("IsDropDownArrowVisible", typeof(bool), typeof(VlvColorPicker), new PropertyMetadata(true));
        #endregion

        #region Is popup opened DP
        public bool IsOpend
        {
            get { return (bool)GetValue(IsOpendProperty); }
            set { SetValue(IsOpendProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpend.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpendProperty =
            DependencyProperty.Register("IsOpend", typeof(bool), typeof(VlvColorPicker), new UIPropertyMetadata(false, OnIsOpenedChanged));

        private static void OnIsOpenedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion

        #region DP for textblock (because cant use globalization from here
        public string TextStandardColors
        {
            get { return (string)GetValue(TextStandardColorsProperty); }
            set { SetValue(TextStandardColorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextStandardColors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextStandardColorsProperty =
            DependencyProperty.Register("TextStandardColors", typeof(string), typeof(VlvColorPicker), new PropertyMetadata(defaultValue:"Colors"));


        #endregion

        #region Arrow color DP


        public Brush ArrowColorBrush
        {
            get { return (Brush)GetValue(ArrowColorBrushProperty); }
            set { SetValue(ArrowColorBrushProperty, value); }
        }

        public static readonly DependencyProperty ArrowColorBrushProperty =
            DependencyProperty.Register("ArrowColorBrush", typeof(Brush), typeof(VlvColorPicker), new PropertyMetadata(Brushes.Black));
        #endregion

        static VlvColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlvColorPicker), new FrameworkPropertyMetadata(typeof(VlvColorPicker)));
        }

        public VlvColorPicker()
        {
            Keyboard.AddKeyDownHandler(this, OnKeyDown);
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseOutsideCapturedElement);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_colorItems != null)
                _colorItems.SelectionChanged -= _colorItems_SelectionChanged;

            _colorItems = GetTemplateChild(PART_ColorItems) as ListBox;

            if (_colorItems != null)
                _colorItems.SelectionChanged += _colorItems_SelectionChanged;

            if (_allColorItems != null)
                _allColorItems.SelectionChanged -= _colorItems_SelectionChanged;

                 _allColorItems = GetTemplateChild(PART_AllColors) as ListBox;

            if (_allColorItems != null)
                _allColorItems.SelectionChanged += _colorItems_SelectionChanged;

            _popup = GetTemplateChild(PART_Popup) as Popup;

            _toggle = GetTemplateChild(PART_ToggleButton) as ToggleButton;
        }

        #region Events
        private void _colorItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;

            if (e.AddedItems.Count > 0)
            {
                var colorItem = e.AddedItems[0] as ColorItem;

                SelectedColor = colorItem.Color;

                lb.SelectedIndex = -1;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            Close();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsOpend)
                IsOpend = true;
            else if (e.Key == Key.Escape)
                Close();

            e.Handled = true;
        }

        private void OnMouseOutsideCapturedElement(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        #endregion

        private void Close()
        {
            if (IsOpend)
                IsOpend = false;

            //if (_toggle != null)
            //    _toggle.Focus();
        }

        #region Helpers
        private static ObservableCollection<ColorItem> CreateStandardColors()
        {
            ObservableCollection<ColorItem> standardColors = new ObservableCollection<ColorItem>();
            standardColors.Add(new ColorItem(Colors.Transparent, "Transparent"));
            standardColors.Add(new ColorItem(Colors.White, "White"));
            standardColors.Add(new ColorItem(Colors.Gray, "Gray"));
            standardColors.Add(new ColorItem(Colors.Black, "Black"));
            standardColors.Add(new ColorItem(Colors.Red, "Red"));
            standardColors.Add(new ColorItem(Colors.Green, "Green"));
            standardColors.Add(new ColorItem(Colors.Blue, "Blue"));
            standardColors.Add(new ColorItem(Colors.Yellow, "Yellow"));
            standardColors.Add(new ColorItem(Colors.Orange, "Orange"));
            standardColors.Add(new ColorItem(Colors.Purple, "Purple"));
            return standardColors;
        }

        private static ObservableCollection<ColorItem> CreateAllColors()
        {
            ObservableCollection<ColorItem> allColors = new ObservableCollection<ColorItem>();

            foreach (var color in GetAllColors())
            {
                var colorItem = new ColorItem(color.Value, color.Key);

                if (!allColors.Contains(colorItem))
                    allColors.Add(colorItem);
            }

            allColors.Remove(allColors[allColors.Count - 1]);

            return allColors;
        }


        private static Dictionary<string, Color> GetAllColors()
        {
            var colorsProps = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
            return colorsProps.ToDictionary(x => x.Name, x => (Color)x.GetValue(null, null));
        }
        #endregion
    }
}
