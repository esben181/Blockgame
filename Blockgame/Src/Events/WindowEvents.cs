using System;

namespace Blockgame.Events
{
    public class WindowResizeEvent : Event
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public WindowResizeEvent(int width, int height)
        {
            Width = width;
            Height = height;
            Console.WriteLine($"Width: {width}, Height: {height}");
        }
    }

    public class WindowClosingEvent : Event
    {
        public WindowClosingEvent()
        {
        }


    }
}
