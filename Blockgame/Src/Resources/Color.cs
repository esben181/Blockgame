
namespace Blockgame.Resources
{
    public partial class Color
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        /// <summary>
        ///  Creates a new RGBA color.
        /// </summary>
        /// <param name="r">Red color channel</param>
        /// <param name="g">Green color channel</param>
        /// <param name="b">Blue color channel</param>
        /// <param name="a">Alpha color channel</param>
        public Color(float r, float g, float b, float a = 255.0f)
        {
            R = r;
            G = g;
            B = b;
            A = a;

        }

        public Color Normalize()
        {
            R = R / 255.0f;
            G = G / 255.0f;
            B = B / 255.0f;
            return this;
        }

        public static Color operator *(Color color, float scalar)
        => new Color(color.R * scalar, color.G * scalar, color.B * scalar);

        public static Color operator -(Color color, float subtractor)
            => new Color(color.R - subtractor, color.G - subtractor, color.B - subtractor);
    }
}
