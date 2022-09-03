using System.Windows;
using System.Windows.Controls;


namespace VlvCustomControlsDotNet.CustomUserControls.RoundedProgressbarControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VlvCustomControlsDotNet.CustomUserControls.RoundedProgressbarControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:VlvCustomControlsDotNet.CustomUserControls.RoundedProgressbarControl;assembly=VlvCustomControlsDotNet.CustomUserControls.RoundedProgressbarControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class VlvRoundedProgressBar : ProgressBar
    {
        private Arc _arc;
        static VlvRoundedProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlvRoundedProgressBar), new FrameworkPropertyMetadata(typeof(VlvRoundedProgressBar)));
        }

        public VlvRoundedProgressBar()
        {
            DefaultStyleKey = typeof(VlvRoundedProgressBar);
            SizeChanged += VlvRoundedProgressBar_SizeChanged;
        }

        private void VlvRoundedProgressBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_arc == null)
                return;
            _arc.Recalculate();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _arc = (Arc)Template.FindName("PART_Arc", this);
        }
    }
}
