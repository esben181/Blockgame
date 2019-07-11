using System;
using OpenTK;


namespace Blockgame
{

    public class Camera
    {
        // yaw = horizontal rotation.
        private float _yaw = (float)-Math.PI/2;
        // pitch = vertical rotation.
        private float _pitch;
        private float _fov = (float)Math.PI/2;

        // Camera orientation vectors.
        private Vector3 _right = Vector3.UnitX;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _front = -Vector3.UnitZ;

        public Vector3 Position { get; set; }
        public float AspectRatio { private get; set; }

        public Vector3 Right => _right;
        public Vector3 Up => _up;
        public Vector3 Front => _front;

        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }
        
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                float angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                CalculateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                CalculateVectors();
            }
        }

        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set => MathHelper.DegreesToRadians(value);
        }
        
        public Matrix4 GetWorldToViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        public Matrix4 GetViewToProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.1f, 100f);
        }

        private void CalculateVectors()
        {
            _front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
            _front.Y = (float)Math.Sin(_pitch);
            _front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);
            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));

            _up = Vector3.Normalize(Vector3.Cross(_right, _front));

        }

    }
}
