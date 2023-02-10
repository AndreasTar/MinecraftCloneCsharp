using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace WindowMain
{
    public class WindowCreator : GameWindow
    {
        public WindowCreator(int width, int height, string title) : base(
            GameWindowSettings.Default, new NativeWindowSettings() {
                Size = (width,height), Title = title 
            }
        ) {}

        public void Run(double fps)
        {
            base.Run();
            base.RenderFrequency = fps;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        }

        protected override void OnUpdateFrame(FrameEventArgs args){
            base.OnUpdateFrame(args);

            KeyboardState input = KeyboardState;
            if(input.IsKeyDown(Keys.Escape)){
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            base.OnResize(args);

            GL.Viewport(0, 0, args.Width, args.Height);
        }
    }
}