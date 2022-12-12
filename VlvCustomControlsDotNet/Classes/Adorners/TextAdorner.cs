using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using static VlvCustomControlsDotNet.TextBoxWithSearch;

namespace VlvCustomControlsDotNet.Classes.Adorners
{
    public class TextAdorner : Adorner
    {
        #region Members
        private VisualCollection visuals;
        private readonly TextBox _textBox;
        private readonly string _text;
        private readonly string _textBoxText;
        private readonly AdornerType _adornerType;
        private readonly Pattern _pattern;

        /// <summary>
        /// Type of adorner
        /// </summary>
        public AdornerType AdornerType => _adornerType;

        /// <summary>
        /// Pattern of this adorner
        /// </summary>
        public Pattern Pattern => _pattern;

        /// <summary>
        /// Occurences
        /// </summary>
        private List<Occurrence> Occurrences { get; set; } = new List<Occurrence>();

        protected override int VisualChildrenCount => visuals.Count;
        #endregion Members


        #region Constructor
        /// <summary>
        /// Constructor for text search
        /// </summary>
        /// <param name="adornedElement"></param>
        /// <param name="text"></param>
        /// <param name="ignoreCase"></param>
        /// <exception cref="NullReferenceException"></exception>
        public TextAdorner(TextBox adornedElement, string text, AdornerType adornerType, string color = "#ff9500", bool ignoreCase = true) : base(adornedElement)
        {
            if (adornedElement == null)
            {
                throw new NullReferenceException("adornedElement");
            }

            visuals = new VisualCollection(this);

            _adornerType = adornerType;
            _pattern = Pattern.Text;

            _textBox = adornedElement;
            _text = ignoreCase ? text.ToLower() : text;
            _textBoxText = ignoreCase ? _textBox.Text.ToLower() : _textBox.Text;

            IsHitTestVisible = false;

            try
            {
                sb = new SolidColorBrush()
                {
                    Color = (Color)ColorConverter.ConvertFromString(color),//("#FF007ACC"),
                    Opacity = 0.5
                };
            }
            catch (Exception)
            {
                sb = new SolidColorBrush()
                {
                    Color = (Color)ColorConverter.ConvertFromString("#ff9500"),//("#FF007ACC"),
                    Opacity = 0.5
                };
            }

            Task.Run(() =>
            {
                int indexFrom = 0;

                while (true)
                {
                    var indexes = Find(indexFrom);

                    if (indexes.start == -2 || indexes.end == -2)
                    {
                        break;
                    }

                    if (indexes.start == -1 || indexes.end == -1)
                    {
                        indexFrom++;
                        continue;
                    }

                    indexFrom = indexes.start == indexes.end ? indexes.end + 1 : indexes.end;
                    Occurrences.Add(new Occurrence(indexes.start, indexes.end));

                    if (indexes.end + _text.Length >= _textBoxText.Length - 1)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            InvalidateVisual();
                        });
                        break;
                    }
                }
            });
        }

        /// <summary>
        /// Constructor for pattern search adorner
        /// </summary>
        public TextAdorner(TextBox adornedElement, int first, int second, Pattern pattern) : base(adornedElement)
        {
            if (adornedElement == null)
            {
                throw new NullReferenceException("adornedElement");
            }

            visuals = new VisualCollection(this);

            _textBox = adornedElement;
            IsHitTestVisible = false;

            _pattern = pattern;
            _adornerType = AdornerType.PatternSearch;

            Task.Run(() =>
            {
                Occurrences.Add(new Occurrence(first, first));
                Occurrences.Add(new Occurrence(second, second));
            });
        }
        #endregion Constructor


        #region Overrides
        private static SolidColorBrush sb = new SolidColorBrush()
        {
            Color = (Color)ColorConverter.ConvertFromString("#ff9500"),//("#FF007ACC"),
            Opacity = 0.5
        };

        protected override void OnRender(DrawingContext drawingContext)
        {
            //System.Diagnostics.Debug.WriteLine("render");

            for (int i = 0; i < Occurrences.Count; i++)
            {
                Rect startPos = _textBox.GetRectFromCharacterIndex(Occurrences[i].StartIndex);
                Rect endPos = _textBox.GetRectFromCharacterIndex(Occurrences[i].EndIndex, true);
                Rect rectUnion = Rect.Union(startPos, endPos);

                drawingContext.DrawRectangle(sb, null, rectUnion);
            }

            base.OnRender(drawingContext);
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }
        #endregion overrides


        #region Helpers
        private (int start, int end) Find(int indexFrom)
        {
            int start = FindStartIndex(indexFrom);

            if (start < 0)
            {
                return (start, -1);
            }

            int endIndex = CheckStringMatch(start);

            if (endIndex < 0)
            {
                return (start, endIndex);
            }

            return (start, endIndex);
        }

        private int FindStartIndex(int from)
        {
            //if (from > _textBoxText.Length - 1)
            //{
            //    return -2;
            //}            
            if (from > _textBoxText.Length - 1)
            {
                return -2;
            }

            if (string.IsNullOrEmpty(_text))
            {
                return -1;
            }

            //for (int i = from; i < _textBox.Text.Length; i++)
            //{
            //    if (_textBox.Text[i] == _text[0])
            //    {
            //        return i;
            //    }
            //}            
            for (int i = from; i < _textBoxText.Length; i++)
            {
                if (_textBoxText[i] == _text[0])
                {
                    return i;
                }
            }

            return -1;
        }

        private int CheckStringMatch(int from)
        {
            //if (from > _textBox.Text.Length - 1)
            //{
            //    return -2;
            //}           
            if (from > _textBoxText.Length - 1)
            {
                return -2;
            }

            if (string.IsNullOrEmpty(_text))
            {
                return -1;
            }

            int _textIndex = 0;
            //for (int i = from; i < _textBox.Text.Length; i++)
            //{
            //    if (_textIndex == _text.Length)
            //    {
            //        return i - 1;
            //    }

            //    if (_textBox.Text[i] == _text[_textIndex])
            //    {
            //        _textIndex++;
            //    }
            //    else
            //    {
            //        return -1;
            //    }
            //}            
            for (int i = from; i < _textBoxText.Length; i++)
            {
                if (_textIndex == _text.Length)
                {
                    return i - 1;
                }

                if (_textBoxText[i] == _text[_textIndex])
                {
                    _textIndex++;
                }
                else
                {
                    return -1;
                }
            }

            if (_textIndex == _text.Length)
            {
                return from + _textIndex - 1;
            }

            return -1;
        }

        struct Occurrence
        {
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }

            public Occurrence(int startIndex, int endIndex) : this()
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }
        }
        #endregion Helpers
    }
}
