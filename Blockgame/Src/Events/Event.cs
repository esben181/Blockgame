using System;

namespace Blockgame.Events
{

    public enum EventType
    {
        KeyPress, KeyRelease,
        WindowClosing, WindowResize
    }
    public class Event
    {

        public bool Handled { get; set; }

    }
}
