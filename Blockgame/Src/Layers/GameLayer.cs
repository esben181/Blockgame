using System;
using OpenTK;
using OpenTK.Input;

using Blockgame.Events;
using Blockgame.Resources;
using Blockgame.World;

namespace Blockgame.Layers
{
    public class GameLayer : Layer
    {

        private Camera _camera;

        private ChunkManager _chunkManager;

        private Vector2 _previousMousePosition;

        public override void Load()
        {
            _chunkManager = new ChunkManager();

            _camera = new Camera(Vector3.Zero, 800 / (float)600);

            _previousMousePosition = Vector2.Zero;
        }

        public override void Update(float deltaTime)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.W))
                _camera.Position += _camera.Front * 5f * deltaTime;
            if (input.IsKeyDown(Key.S))               
                _camera.Position -= _camera.Front * 5f * deltaTime;
            if (input.IsKeyDown(Key.D))               
                _camera.Position += _camera.Right * 5f * deltaTime;
            if (input.IsKeyDown(Key.A))               
                _camera.Position -= _camera.Right * 5f * deltaTime;

            
            var mouse = Mouse.GetState();
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                _chunkManager.DestroyBlock(_camera.Position);
            }
            if (mouse.IsButtonDown(MouseButton.Right))
            {
                _chunkManager.PlaceBlock(BlockType.Dirt, _camera.Position);
            }
            if (input.IsKeyUp(Key.AltLeft))
            {
                float deltaX = mouse.X - _previousMousePosition.X;
                float deltaY = mouse.Y - _previousMousePosition.Y;
                _previousMousePosition = new Vector2(mouse.X, mouse.Y);

                _camera.Pitch -= deltaY * 0.2f;
                _camera.Yaw += deltaX * 0.2f;

            }
        }

        public override void OnEvent(Event @event)
        {
            if (@event is WindowResizeEvent resizeEvent)
            {
                _camera.AspectRatio = resizeEvent.Width / (float)resizeEvent.Height;
            }
        }

        public override void Render()
        {

            _chunkManager.Render(_camera);
            

        }

        public override void Unload()
        {
            _chunkManager.Dispose();

        }
    }
}
