using Blockgame.Windowing;

namespace Blockgame
{
    public class Program
    {
        static void Main()
        {
            using (var game = new Window(800, 600, "Blockgame"))
            {
                game.Run();
            }
        }
    }
}
