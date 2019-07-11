using System;
using Blockgame.Windowing;

namespace Blockgame
{
    class Program
    {
        static void Main()
        {
            using (var game = new Window(600, 800, "Blockgame"))
            {
                game.Run();
            }
        }
    }
}
