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

        private Shader _shader;
        private TextureArray _textureArray;
        private Camera _camera;

        private Chunk _firstChunk;
        private Chunk _secondChunk;

        private bool _firstMove = true;
        private Vector2 _previousPosition;

        public override void Load()
        {
            _camera = new Camera(Vector3.Zero, 800 / (float)600);
            _firstChunk = new Chunk();
            _secondChunk = new Chunk();

            _shader = new Shader("Shaders/block.vert", "Shaders/block.frag");
            _shader.Bind();

            _textureArray = new TextureArray("Textures/map.png", 128, 128, Enum.GetNames(typeof(BlockType)).Length);
            _textureArray.Bind();
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
            if (_firstMove)
            {
                _previousPosition = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else if (input.IsKeyUp(Key.AltLeft))
            {
                float deltaX = mouse.X - _previousPosition.X;
                float deltaY = mouse.Y - _previousPosition.Y;
                _previousPosition = new Vector2(mouse.X, mouse.Y);

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

            _textureArray.Bind();
            _shader.Bind();

            _shader.SetMatrix4("u_view", _camera.GetWorldToViewMatrix());
            _shader.SetMatrix4("u_projection", _camera.GetViewToProjectionMatrix());
            _shader.SetVector3("u_lightPos", _camera.Position);
            _shader.SetMatrix4("u_model", Matrix4.Identity);
            _shader.SetInt("u_texture", 0);
            _firstChunk.Render();
            _shader.SetMatrix4("u_model", Matrix4.Identity * Matrix4.CreateTranslation(new Vector3(Chunk.Size, 0,  0)));
            _secondChunk.Render();

        }

        public override void Unload()
        {
            _shader.Dispose();
            _textureArray.Dispose();
            _firstChunk.Dispose();
            _secondChunk.Dispose();

        }
    }
}
