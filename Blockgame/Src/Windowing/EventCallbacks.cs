using System;
using System.ComponentModel;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;

using Blockgame.Events;

namespace Blockgame.Windowing
{

    public partial class Window
    {

        public Action<Event> OnEventCallback;

        public void OnEvent(Event @event)
        {
            _layerStack.OnEvent(@event);

        }
        protected override void OnResize(EventArgs @event)
        {
            GL.Viewport(0, 0, Width, Height);
            OnEventCallback(new WindowResizeEvent(Width, Height));
        }
        
        protected override void OnMouseMove(MouseMoveEventArgs @event)
        {

        }
        protected override void OnKeyDown(KeyboardKeyEventArgs @event)
        {
            OnEventCallback(new KeyPressEvent(@event.Key));
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs @event)
        {
            OnEventCallback(new KeyReleaseEvent(@event.Key));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            OnEventCallback(new WindowClosingEvent());
            _layerStack.RemoveAll();
        }
    }
}
