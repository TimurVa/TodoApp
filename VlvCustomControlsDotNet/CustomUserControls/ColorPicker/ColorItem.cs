using System.Windows.Media;

namespace VlvCustomControlsDotNet
{
    public class ColorItem
    {
        public Color Color { get; set; }

        public string Name { get; set; }

        public ColorItem(Color color, string colorName)
        {
            Color = color;
            Name = colorName;
        }
    }
}
