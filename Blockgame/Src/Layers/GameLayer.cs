using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;

using Blockgame.Events;
using Blockgame.Resources;
using Blockgame.World;

namespace Blockgame.Layers
{
    public class GameLayer : Layer
    {

        private Camera _camera;

        private Map _chunkManager;

        private Vector2 _previousMousePosition;

        bool _wireFrameMode = false;
        BlockKind _buildingBlock = BlockKind.Grass;

        public override void Load()
        {
            _chunkManager = new Map();

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

            if (input.IsKeyDown(Key.F3))
            {
                // Normal fill mode
                if (!_wireFrameMode) 
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                }
                else
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                }
                _wireFrameMode = !_wireFrameMode;
            }
            if (input.IsKeyDown(Key.Number1))
            {
                _buildingBlock = BlockKind.Grass;
            }
            else if (input.IsKeyDown(Key.Number2))
            {
                _buildingBlock = BlockKind.Wood;
            }
            else if (input.IsKeyDown(Key.Number3))
            {
                _buildingBlock = BlockKind.Stone;
            }
            else if (input.IsKeyDown(Key.Number4))
                _buildingBlock = BlockKind.Mushroom;
            else if (input.IsKeyDown(Key.Number5))
                _buildingBlock = BlockKind.MushroomStem;

            var mouse = Mouse.GetState();
            var target = _camera.Position + (_camera.Front * 2.0f);
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                _chunkManager.DestroyBlock(target);
            }
            if (mouse.IsButtonDown(MouseButton.Right))
            {
                _chunkManager.PlaceBlock(_buildingBlock, target);
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
