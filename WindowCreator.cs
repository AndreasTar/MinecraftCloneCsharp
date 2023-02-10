using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;
using Camera;

namespace WindowMain
{
    public class WindowCreator : GameWindow
    {
        float[] verts = {
            // front
            -0.5f, -0.5f, 0.5f, // bottom left
            0.5f, -0.5f, 0.5f,  // bottom right
            -0.5f, 0.5f, 0.5f,  // top left
            0.5f, 0.5f, 0.5f,   // top right
            // back
            -0.5f, -0.5f, -0.5f,// bot left
            0.5f, -0.5f, -0.5f, // bot right
            -0.5f, 0.5f, -0.5f, // top left
            0.5f, 0.5f, -0.5f,  // top right

        };

        uint[] indices = {
            0, 3, 1, // front
            0, 2, 3,
            5, 7, 6, // back
            5, 6, 4,
            0, 4, 6, // left side
            0, 6, 2,
            1, 3, 5, // right side
            5, 3, 7,
            0, 1, 5, // bottom
            5, 4, 0,
            3, 2, 6, // top
            3, 6, 7

        };

        int vbo;
        int ebo;
        int vao;
        Shader shader;

        private double timeval;
        KeyboardState keyb;
        MouseState mous;
        CameraControl camera;

        Matrix4 View, Projection;
        

        public WindowCreator(int width, int height, string title) : base(
            GameWindowSettings.Default, new NativeWindowSettings() {
                Size = (width,height), Title = title 
            }
        ) {
            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            keyb = KeyboardState;
            mous = MouseState;
            camera = new CameraControl(Vector3.UnitZ * 3, Size.X / (float) Size.Y, keyb, mous);
            CursorState = CursorState.Grabbed;
        }

        public void Run(double fps)
        {
            base.Run();
            base.RenderFrequency = fps;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length*sizeof(float), verts, BufferUsageHint.StaticDraw);
            // StaticDraw : data will most likely never change or change very rarely
            // DynamicDraw: data will change frequently
            // StreamDraw : data will change every time its drawn

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length*sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3*sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            View = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X/(float)Size.Y, 0.1f, 100.0f);


            shader.Use();
            // draw the object now
        }

        protected override void OnUnload()
        {
            /*
                NOTE
                you dont have to delete buffers etc, the programm does by itself when it closes.
                But in case you want to limit VRAM usage etc, you can always delete them by yourself using
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(vbo);
                any calls that modify the buffer set to 0 -> null, will result in a crash, so easy to debug.
            */
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(ebo);

            base.OnUnload();

            shader.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs args){
            base.OnUpdateFrame(args);

            if(!IsFocused) return;

            if(keyb.IsKeyDown(Keys.Escape)){
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(vao);
            shader.Use();

            timeval += 6.0f * args.Time;
            float greenval = (float)Math.Sin(timeval/6.0f)/2.0f+0.5f;
            int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "ourColor");
            GL.Uniform4(vertexColorLocation, 0.0f, greenval, 0.0f, 1.0f);

            Matrix4 Model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(timeval));
            Model *= Matrix4.Identity * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(timeval));

            shader.SetMatrix4("model", Model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            camera.OnMouseWheel(e);
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            base.OnResize(args);

            GL.Viewport(0, 0, args.Width, args.Height);
        }
    }
}