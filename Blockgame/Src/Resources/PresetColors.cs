using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockgame.Resources
{
    public partial class Color
    {
        public static Color White { get => new Color(255.0f, 255.0f, 255.0f); }
        public static Color Black { get => new Color(0.0f, 0.0f, 0.0f); }
        public static Color Red { get => new Color(255.0f, 0.0f, 0.0f); }
        public static Color CrimsonRed { get => new Color(153.0f, 0.0f, 0.0f); }
        public static Color Pink { get => new Color(255.0f, 0.0f, 220.0f); }
        public static Color Purple { get => new Color(178.0f, 0.0f, 255.0f); }
        public static Color Green { get => new Color(0.0f, 255.0f, 0.0f); }
        public static Color LawnGreen { get => new Color(124.0f, 252.0f, 0.0f); }
        public static Color Yellow { get => new Color(255.0f, 255.0f, 0.0f); }
        public static Color Olive { get => new Color(128.0f, 128.0f, 0.0f); }
        public static Color Orange { get => new Color(255.0f, 154.0f, 0.0f); }
        public static Color Brown { get => new Color(124.0f, 81.0f, 52.0f); }
        public static Color Gray { get => new Color(139.0f, 134.0f, 130.0f); }
        public static Color Blue { get => new Color(0.0f, 0.0f, 255.0f); }
        public static Color Turquoise { get => new Color(64.0f, 224.0f, 208.0f); }
        public static Color CornflowerBlue { get => new Color(142.0f, 167.0f, 255.0f); }
    }
}
