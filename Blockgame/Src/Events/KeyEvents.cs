using OpenTK;
using OpenTK.Input;

namespace Blockgame.Events
{
    public class KeyPressEvent : Event
    {
         public Key Key { get; private set; }
        public KeyPressEvent(Key keyCode)
        {
            Key = keyCode;
        }

    }

    public class KeyReleaseEvent : Event
    {
        public Key Key { get; private set; }
        public KeyReleaseEvent(Key keyCode)
        {
            Key = keyCode;
        }

    }
}
