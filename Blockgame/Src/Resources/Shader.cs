using System;
using System.IO;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Blockgame.Resources
{

    public class Shader : IDisposable
    {
        private int _programId;

        private bool _disposed = false;

        public void Bind()
        {
            GL.UseProgram(_programId);
        }

        public Shader(string vertexPath, string fragmentPath)
        {
            // Create vertex shader
            var shaderSource = LoadSource(vertexPath);
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, shaderSource);
            CompileShader(vertexShader);


            //  Create fragment shader
            shaderSource = LoadSource(fragmentPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            // Create shader program
            _programId = GL.CreateProgram();
            GL.AttachShader(_programId, vertexShader);
            GL.AttachShader(_programId, fragmentShader);
            LinkProgram(_programId);


            // Free resources
            GL.DetachShader(_programId, vertexShader);
            GL.DetachShader(_programId, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

        }

        private static string LoadSource(string path)
        {
            using (var file = new StreamReader(path, Encoding.UTF8))
            {
                return file.ReadToEnd();
            }
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            string infoLog = GL.GetShaderInfoLog(shader);
            if (infoLog != String.Empty)
            {
                Console.WriteLine(infoLog);
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            string infoLog = GL.GetProgramInfoLog(program);
            if (infoLog != String.Empty)
            {
                Console.WriteLine(infoLog);
            }
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(_programId);
            GL.UniformMatrix4(GL.GetUniformLocation(_programId, name), false, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(_programId);
            GL.Uniform3(GL.GetUniformLocation(_programId, name), data);
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(_programId);
            GL.Uniform1(GL.GetUniformLocation(_programId, name), data);
        }


        ~Shader()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;


            GL.DeleteProgram(_programId);

            _disposed = true;
        }
    }
}