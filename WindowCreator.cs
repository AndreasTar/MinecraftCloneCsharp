using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;
using Camera;

namespace GameMain
{
    public class Game : GameWindow
    {
        // float[] verts = {
        //     // front
        //     -0.5f, -0.5f, 0.5f, // bottom left
        //     0.5f, -0.5f, 0.5f,  // bottom right
        //     -0.5f, 0.5f, 0.5f,  // top left
        //     0.5f, 0.5f, 0.5f,   // top right
        //     // back
        //     -0.5f, -0.5f, -0.5f,// bot left
        //     0.5f, -0.5f, -0.5f, // bot right
        //     -0.5f, 0.5f, -0.5f, // top left
        //     0.5f, 0.5f, -0.5f,  // top right

        // };

        // uint[] indices = {
        //     0, 3, 1, // front
        //     0, 2, 3,
        //     5, 7, 6, // back
        //     5, 6, 4,
        //     0, 4, 6, // left side
        //     0, 6, 2,
        //     1, 3, 5, // right side
        //     5, 3, 7,
        //     0, 1, 5, // bottom
        //     5, 4, 0,
        //     3, 2, 6, // top
        //     3, 6, 7

        // };

        int vboPos, vboCol, vboMview, ebo;
        int attribVPos, attribVCol, attribModel, attribView, attribProjection;
        Vector3[] vertData, colorData;
        int[] indexData;

        Shader shader;
        KeyboardState keyb;
        MouseState mous;
        CameraControl camera;

        private double deltaTime;
        private double timeVal;

        List<Volume> objects = new List<Volume>();

        //Matrix4 View, Projection;
        

        public Game(int width, int height, string title) : base(
            GameWindowSettings.Default, new NativeWindowSettings() {
                Size = (width,height), Title = title , NumberOfSamples = 4
            }
        ) {
            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            keyb = KeyboardState;
            mous = MouseState;
            camera = new CameraControl(Vector3.UnitZ * 3, Size.X / (float) Size.Y, keyb, mous);
            CursorState = CursorState.Grabbed;
        }

        public void Run(double fps, int tps)
        {
            //base.VSync = VSyncMode.Adaptive;
            if(fps > 0) base.RenderFrequency = fps;
            if(tps > 0) base.UpdateFrequency = tps;
            base.Run();
        }

        void initProgram(){
            objects.Add(new Block());
            
            attribVPos = shader.GetAttribLocation("vPosition");
            attribVCol = shader.GetAttribLocation("vColor");
            attribModel = shader.GetUniformLocation("model");
            attribView = shader.GetUniformLocation("view");
            attribProjection = shader.GetUniformLocation("projection");

            vboPos = GL.GenBuffer();
            vboCol = GL.GenBuffer();
            vboMview = GL.GenBuffer();
            ebo = GL.GenBuffer();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            initProgram();

            GL.ClearColor(Color4.CornflowerBlue);
            //GL.ClearDepth(0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
           // GL.CullFace(CullFaceMode.Back);
            GL.PointSize(5f);

            shader.Use();
            // draw the object now;
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
            GL.DeleteBuffer(vboPos);
            GL.DeleteBuffer(vboCol);
            GL.DeleteBuffer(vboMview);
            //GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(ebo);

            base.OnUnload();

            shader.Dispose();
        }

        /*
            This is basically the Updating part of the whole system.
            It is called once everytime the programm needs to be updated, and only manages TPS.
            Its one part of the decoupling between FPS and TPS in games, this being the TPS.
            Look to OnRenderFrame for the FPS.
            All actions regarding updating the game (like controls, AI etc) should be inserted here.
        */
        protected override void OnUpdateFrame(FrameEventArgs args){
            base.OnUpdateFrame(args);

            if(!IsFocused) return;

            if(keyb.IsKeyDown(Keys.Escape)){
                Console.WriteLine("{0}    {1}    {2}    {3}\n", camera.cameraPosition, camera.cameraFov, camera.cameraFront, camera.cameraYaw);
                Close();
            }
            camera.ManageInput(args);
        }

        /*
            This is basically the Rendering part of the whole system.
            It is called once everytime the image can be rendered, and only manages FPS.
            Its one part of the decoupling between FPS and TPS in games, this being the FPS.
            Look to OnUpdateFrame for the TPS.
            All actions regarding rendering the game (like shaders, effects etc) should be inserted here.
        */
        protected override void OnRenderFrame(FrameEventArgs args) 
        {
            base.OnRenderFrame(args);
            deltaTime = args.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            List<Vector3> vertices = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();

            int vertcount = 0;
            foreach (Volume v in objects)
            {
                vertices.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                vertcount += v.VertCount;
            }

            vertData = vertices.ToArray();
            indexData = inds.ToArray();
            colorData = colors.ToArray();

            // StaticDraw : data will most likely never change or change very rarely
            // DynamicDraw: data will change frequently
            // StreamDraw : data will change every time its drawn
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboPos);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Length * Vector3.SizeInBytes), vertData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribVPos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboCol);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colorData.Length * Vector3.SizeInBytes), colorData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribVCol, 3, VertexAttribPointerType.Float, false, 0, 0);

            timeVal += 6.0f * deltaTime;
            objects[0].Rotation = new Vector3(0.55f * (float)timeVal, 0.25f * (float)timeVal,0);

            foreach (Volume v in objects)
            {
                v.CalculateModelMatrix();

            }

            shader.Use();
  
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexData.Length * sizeof(int)), indexData, BufferUsageHint.StaticDraw);


            GL.EnableVertexAttribArray(attribVPos);
            GL.EnableVertexAttribArray(attribVCol);

            //GL.BindVertexArray(vao);

            int indexat = 0;
            foreach (Volume v in objects)
            {
                shader.SetMatrix4("model", v.ModelMatrix);
                GL.DrawElements(PrimitiveType.Triangles, v.IndexCount, DrawElementsType.UnsignedInt, indexat * sizeof(uint));
                indexat += v.IndexCount;
            }

            GL.DisableVertexAttribArray(attribVPos);
            GL.DisableVertexAttribArray(attribVCol);

            //int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "ourColor");
            //GL.Uniform4(vertexColorLocation, 0.0f, greenval, 0.0f, 1.0f);

            // Matrix4 Model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(timeVal));

            // shader.SetMatrix4("model", Model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.Flush();
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
            camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }
}