using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using Blockgame.Layers;

namespace Blockgame.Windowing
{

    public partial class Window : OpenTK.GameWindow
    {

        private LayerStack _layerStack;

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
            _layerStack = new LayerStack();

            OnEventCallback = OnEvent;
        }

        protected override void OnLoad(EventArgs e)
        {
            
            GL.ClearColor(0.96f, 0.16f, 0.91f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _layerStack.AddLayer(new GameLayer());
            
        }



        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            _layerStack.Update((float)e.Time);
           

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _layerStack.Render();
            
            SwapBuffers();

        }

    }
}
