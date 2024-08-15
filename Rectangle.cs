using SFML.Graphics.Glsl;

namespace To_The_Stars_hashtag_version
{
    internal class Rectangle
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle(double left, double top, double width, double height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
        public Rectangle() { }
        public bool Intersects(Rectangle other)
        {
            if (Left + Width < other.Left || other.Left + other.Width < Left)
            {
                return false;
            }
            if (Top + Height < other.Top || other.Top + other.Height < Top)
            {
                return false;
            }

            return true;
        }
    }
}
