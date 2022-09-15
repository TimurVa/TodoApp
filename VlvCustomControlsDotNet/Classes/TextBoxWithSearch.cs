using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using VlvCustomControlsDotNet.Classes.Adorners;

namespace VlvCustomControlsDotNet
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    public class TextBoxWithSearch : Control
    {
        #region Private members
        private TextBox _textBox;
        #endregion


        #region Members
        public enum Pattern
        {
            Text,
            Parentheses,
            Braces,
            SquareBraces
        }

        public enum AdornerType
        {
            TextSearch,
            PatternSearch
        }
        #endregion


        #region Search text DP
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(TextBoxWithSearch), new PropertyMetadata(string.Empty, OnSearchTextChanged));

        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxWithSearch t = d as TextBoxWithSearch;
            t.SearchTextChanged(e);
        }

        private void SearchTextChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_textBox == null)
            {
                return;
            }

            string searchText = e.NewValue.ToString();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(_textBox);

            if (adornerLayer != null)
            {
                RemoveAdorner(adornerLayer, AdornerType.TextSearch, Pattern.Text);

                TextAdorner textAdorner = new TextAdorner(_textBox, searchText);
                if (textAdorner != null)
                {
                    adornerLayer.Add(textAdorner);
                }
            }
        }
        #endregion


        #region Text DP
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithSearch), new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion


        #region Pattern DP
        public IList<Pattern> StartEndPatterns
        {
            get { return (IList<Pattern>)GetValue(StartEndPatternsProperty); }
            set { SetValue(StartEndPatternsProperty, value); }
        }

        public static readonly DependencyProperty StartEndPatternsProperty =
            DependencyProperty.Register("StartEndPatterns", typeof(IList<Pattern>), typeof(TextBoxWithSearch), new PropertyMetadata(null, OnPatternChanged));

        private static void OnPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxWithSearch textBoxWithSearch = (TextBoxWithSearch)d;
            textBoxWithSearch.PatternChanged(e);
        }

        private void PatternChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == null && e.NewValue != null)
            {
                _textBox.SelectionChanged += _textBox_SelectionChanged;
            }
            if (e.OldValue != null && e.NewValue == null)
            {
                _textBox.SelectionChanged -= _textBox_SelectionChanged;
            }
        }

        private void _textBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_textBox.Text))
            {
                return;
            }

            for (int i = 0; i < StartEndPatterns.Count; i++)
            {
                if (StartEndPatterns[i] == Pattern.Parentheses)
                {
                    TryCreatePatternAdorner('(', ')', Pattern.Parentheses);
                }
                else if (StartEndPatterns[i] == Pattern.Braces)
                {
                    TryCreatePatternAdorner('{', '}', Pattern.Braces);
                }
                else if (StartEndPatterns[i] == Pattern.SquareBraces)
                {
                    TryCreatePatternAdorner('[', ']', Pattern.SquareBraces);
                }
            }
        }

        private void TryCreatePatternAdorner(char start, char end, Pattern pattern)
        {
            var result = GetPattern(_textBox.Text, _textBox.CaretIndex, start, end);

            if (result == (-1, -1))
            {
                RemoveAdorner(AdornerLayer.GetAdornerLayer(_textBox), AdornerType.PatternSearch, pattern);
                return;
            }

            CreateAdorner(result.first, result.second, pattern);
        }
        #endregion


        static TextBoxWithSearch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxWithSearch), new FrameworkPropertyMetadata(typeof(TextBoxWithSearch)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBox = Template.FindName("PART_TextBox", this) as TextBox;

            _textBox.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                if (e.Key == Key.Escape)
                {
                    var thisBinding = GetBindingExpression(TextProperty);

                    var binding = _textBox.GetBindingExpression(TextBox.TextProperty);

                    if (binding != null && thisBinding != null && thisBinding.ParentBinding != null && thisBinding.ParentBinding is System.Windows.Data.Binding b)
                    {
                        if (b.UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.LostFocus || b.UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.Default)
                        {
                            binding.UpdateTarget();
                            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(_textBox), null);
                            Keyboard.ClearFocus();
                        }
                    }
                }
            }
        }


        #region Adorner helpers
        /// <summary>
        /// Only for pattern adorner
        /// </summary>
        private void CreateAdorner(int startIndex, int endIndex, Pattern pattern)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(_textBox);

            if (adornerLayer != null)
            {
                RemoveAdorner(adornerLayer, AdornerType.PatternSearch, pattern);
                TextAdorner textAdorner = new TextAdorner(_textBox, startIndex, endIndex, pattern);

                if (textAdorner != null)
                {
                    adornerLayer.Add(textAdorner);
                }
            }
        }

        /// <summary>
        /// Removes all adorners which has desired <see cref="AdornerType"/>
        /// and <see cref="Pattern"/>
        /// </summary>
        private void RemoveAdorner(AdornerLayer adornerLayer, AdornerType adornerType, Pattern pattern)
        {
            if (adornerLayer != null)
            {
                var toRemove = adornerLayer.GetAdorners(_textBox);
                if (toRemove != null)
                {
                    for (int i = 0; i < toRemove.Length; i++)
                    {
                        if (toRemove[i] is TextAdorner t)
                        {
                            if (t.AdornerType == adornerType && t.Pattern == pattern)
                            {
                                adornerLayer.Remove(toRemove[i]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets start and end index of start and end char or (-1,-1) if no occurences exist
        /// </summary>
        private (int first, int second) GetPattern(string text, int startIndex, char startChar, char endChar)
        {
            if (string.IsNullOrEmpty(text))
            {
                return (-1, -1);
            }

            if (startIndex == 0 && text[startIndex] != startChar)
            {
                return (-1, -1);
            }

            if (text.Length - 1 < startIndex)
            {
                if (text[text.Length - 1] == endChar)
                {
                    var result = GetPrevOpenPattern(startIndex - 2, text, startChar, endChar);

                    if (result != -1)
                    {
                        return (text.Length - 1, result);
                    }
                }

                return (-1, -1);
            }

            if (text[startIndex] == startChar)
            {
                var result = GetNextClosePattern(startIndex + 1, text, startChar, endChar);

                if (result != -1)
                {
                    return (startIndex, result);
                }

                return (-1, -1);
            }

            if (text[startIndex - 1] >= 0 && text[startIndex - 1] == endChar)
            {
                var result = GetPrevOpenPattern(startIndex - 2, text, startChar, endChar);

                if (result != -1)
                {
                    return (startIndex - 1, result);
                }
            }

            return (-1, -1);
        }

        /// <returns>Index of last occurrence of starChar</returns>
        private int GetNextClosePattern(int start, string text, char startChar, char endChar)
        {
            int countOfOpened = 1;
            for (int i = start; i < text.Length; i++)
            {
                if (text[i] == startChar)
                {
                    countOfOpened++;
                    continue;
                }

                if (text[i] == endChar)
                {
                    countOfOpened--;

                    if (countOfOpened == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <returns>
        /// Index of last occurence of endChar
        /// </returns>
        private int GetPrevOpenPattern(int end, string text, char startChar, char endChar)
        {
            int countOfClosed = 1;
            for (int i = end; i >= 0; i--)
            {
                if (text[i] == endChar)
                {
                    countOfClosed++;
                    continue;
                }

                if (text[i] == startChar)
                {
                    countOfClosed--;

                    if (countOfClosed == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
        #endregion
    }
}
