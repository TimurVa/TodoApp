using System.Windows.Controls;
using System.Windows.Documents;
using ToDoApp.Helpers.Adorners;

namespace ToDoApp.Pages
{
    /// <summary>
    /// Interaction logic for TodoPage.xaml
    /// </summary>
    public partial class TodoPage : Page
    {
        public TodoPage()
        {
            InitializeComponent();
        }

        //private void tb2_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(tb);

        //    if (adornerLayer != null)
        //    {
        //        RemoveAdorner(false, adornerLayer);

        //        TextAdorner textAdorner = new TextAdorner(tb, tb2.Text);
        //        if (textAdorner != null)
        //        {
        //            adornerLayer.Add(textAdorner);
        //        }
        //    }
        //}

        //private void tb_SelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    RemoveAdorner(true, AdornerLayer.GetAdornerLayer(tb));

        //    if (string.IsNullOrEmpty(tb.Text))
        //    {
        //        return;
        //    }

        //    var result = GetParenthesses(tb.Text, tb.CaretIndex);

        //    if (result == (-1, -1))
        //    {
        //        return;
        //    }

        //    CreateAdorner(result.first, result.second);

        //    //if (tb.Text.Length - 1 < tb.CaretIndex)
        //    //{
        //    //    if (tb.Text[tb.Text.Length - 1] == ')')
        //    //    {
        //    //        var result = GetPrevOpenParentheses(tb.CaretIndex - 2, tb.Text);

        //    //        if (result != -1)
        //    //        {
        //    //            CreateAdorner(tb.Text.Length - 1, result);
        //    //        }
        //    //    }
        //    //    return;
        //    //}

        //    //if (tb.Text[tb.CaretIndex] == '(')
        //    //{
        //    //    var result = GetNextCloseParentheses(tb.CaretIndex + 1, tb.Text);

        //    //    if (result != -1)
        //    //    {
        //    //        CreateAdorner(tb.CaretIndex, result);
        //    //    }
        //    //}
        //    //else if (tb.CaretIndex - 1 >= 0 && tb.Text[tb.CaretIndex - 1] == ')')
        //    //{
        //    //    var result = GetPrevOpenParentheses(tb.CaretIndex - 2, tb.Text);

        //    //    if (result != -1)
        //    //    {
        //    //        CreateAdorner(tb.CaretIndex - 1, result);
        //    //    }
        //    //}
        //}

        //private (int first, int second) GetParenthesses(string text, int startIndex)
        //{
        //    if (string.IsNullOrEmpty(text))
        //    {
        //        return (-1, -1);
        //    }

        //    if (startIndex == 0 && text[startIndex] != '(')
        //    {
        //        return (-1, -1);
        //    }

        //    if (text.Length - 1 < startIndex)
        //    {
        //        if (text[text.Length - 1] == ')')
        //        {
        //            var result = GetPrevOpenParentheses(startIndex - 2, text);

        //            if (result != -1)
        //            {
        //                return (text.Length - 1, result);
        //            }
        //        }

        //        return (-1, -1);
        //    }

        //    if (text[startIndex] == '(')
        //    {
        //        var result = GetNextCloseParentheses(startIndex + 1, text);

        //        if (result != -1)
        //        {
        //            return (startIndex, result);
        //        }

        //        return (-1, -1);
        //    }

        //    if (text[startIndex - 1] >= 0 && text[startIndex - 1] == ')')
        //    {
        //        var result = GetPrevOpenParentheses(startIndex - 2, text);

        //        if (result != -1)
        //        {
        //            return (startIndex - 1, result);
        //        }
        //    }

        //    return (-1, -1);
        //}

        //private int GetNextCloseParentheses(int start, string text)
        //{
        //    int countOfOpened = 1;
        //    for (int i = start; i < text.Length; i++)
        //    {
        //        if (text[i] == '(')
        //        {
        //            countOfOpened++;
        //            continue;
        //        }

        //        if (text[i] == ')')
        //        {
        //            countOfOpened--;

        //            if (countOfOpened == 0)
        //            {
        //                return i;
        //            }
        //        }
        //    }

        //    return -1;
        //}

        //private int GetPrevOpenParentheses(int end, string text)
        //{
        //    int countOfClosed = 1;
        //    for (int i = end; i >= 0; i--)
        //    {
        //        if (text[i] == ')')
        //        {
        //            countOfClosed++;
        //            continue;
        //        }

        //        if (text[i] == '(')
        //        {
        //            countOfClosed--;

        //            if (countOfClosed == 0)
        //            {
        //                return i;
        //            }
        //        }
        //    }

        //    return -1;
        //}

        //private void CreateAdorner(int first, int second)
        //{
        //    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(tb);

        //    if (adornerLayer != null)
        //    {
        //        RemoveAdorner(true, adornerLayer);

        //        TextAdorner textAdorner = new TextAdorner(tb, first, second);
        //        if (textAdorner != null)
        //        {
        //            adornerLayer.Add(textAdorner);
        //        }
        //    }
        //}

        //private void RemoveAdorner(bool parentheses, AdornerLayer adornerLayer)
        //{
        //    if (adornerLayer == null)
        //    {
        //        return;
        //    }

        //    var toRemove = adornerLayer.GetAdorners(tb);

        //    if (toRemove == null)
        //    {
        //        return;
        //    }

        //    for (int i = 0; i < toRemove.Length; i++)
        //    {
        //        if (toRemove[i] is not TextAdorner ta)
        //        {
        //            continue;
        //        }

        //        if (parentheses && ta.IsParenthesesAdorner)
        //        {
        //            adornerLayer.Remove(toRemove[i]);
        //            continue;
        //        }

        //        if (!parentheses && !ta.IsParenthesesAdorner)
        //        {
        //            adornerLayer.Remove(toRemove[i]);
        //            continue;
        //        }
        //    }
        //}
    }
}
