using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

namespace ToDoApp.Helpers.Attached
{
    class TextBoxAttachedProperties
    {


        public static bool GetBindingOnKeyUp(DependencyObject obj)
        {
            return (bool)obj.GetValue(BindingOnKeyUpProperty);
        }

        public static void SetBindingOnKeyUp(DependencyObject obj, bool value)
        {
            obj.SetValue(BindingOnKeyUpProperty, value);
        }

        // Using a DependencyProperty as the backing store for BindingOnKeyUp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingOnKeyUpProperty =
            DependencyProperty.RegisterAttached("BindingOnKeyUp", 
                typeof(bool), 
                typeof(TextBoxAttachedProperties),
                new PropertyMetadata(false, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = d as TextBox;

            if ((bool)e.OldValue == false && e.NewValue != null)
            {
                tb.KeyUp += Tb_KeyUp;
                tb.PreviewKeyDown += Tb_PreviewKeyDown;
            }
            else if ((bool)e.OldValue != false && e.NewValue == null)
            {
                tb.KeyUp -= Tb_KeyUp;
                tb.PreviewKeyDown -= Tb_PreviewKeyDown;
            }
        }

        private static void Tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            {
                e.Handled = true;
            }
        }

        private static void Tb_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (e.Key == Key.Escape || e.Key == Key.Enter 
                //&& (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control
                && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
            {
                var binding = tb.GetBindingExpression(TextBox.TextProperty);

                if (e.Key == Key.Escape)
                {
                    if (binding != null && binding.ParentBinding != null && binding.ParentBinding is System.Windows.Data.Binding b)
                    {
                        if (b.UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.LostFocus || b.UpdateSourceTrigger == UpdateSourceTrigger.Default)
                        {
                            binding.UpdateTarget();
                        }
                    }
                }
                else
                {
                    if (binding != null && binding.ParentBinding != null && binding.ParentBinding is System.Windows.Data.Binding b)
                    {
                        if (b.UpdateSourceTrigger == System.Windows.Data.UpdateSourceTrigger.LostFocus || b.UpdateSourceTrigger == UpdateSourceTrigger.Default)
                        {
                            binding.UpdateSource();
                        }
                    }
                }

                Keyboard.ClearFocus();
            }
        }
    }
}
