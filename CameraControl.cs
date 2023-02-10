using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System;

namespace Camera
{
    public class CameraControl{
        
        Vector3 up = Vector3.UnitY;
        Vector3 front = -Vector3.UnitZ;
        Vector3 right = Vector3.UnitX;
        float pitch;
        float yaw = -MathHelper.PiOver2;
        float fov = 80f;
        //float fov = MathHelper.PiOver2;

        public Vector3 cameraPosition {get; set;}
        public float AspectRatio {private get; set;}
        public Vector3 cameraFront => front;
        public Vector3 cameraUp => up;
        public Vector3 cameraRight => right;

        float speed;
        float sensitivity;
        KeyboardState keyboard;
        MouseState mouse;

        private bool firstMove = true;
        private Vector2 lastMousePos;

        public CameraControl(Vector3 pos, float aspectRatio, KeyboardState keyb, MouseState mous){
            cameraPosition = pos;
            AspectRatio = aspectRatio;
            keyboard = keyb;
            mouse = mous;
        }

        public float cameraPitch{
            get => MathHelper.RadiansToDegrees(pitch);
            set {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float cameraYaw
        {
            get => MathHelper.RadiansToDegrees(yaw);
            set
            {
                yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }
        public float cameraFov
        {
            
            get => MathHelper.DegreesToRadians(fov);
            set
            {
                //float angle = MathHelper.Clamp(value, -20f, 20f);
                //fov = MathHelper.DegreesToRadians(angle);
                //fov = MathHelper.Clamp(fov, MathHelper.DegreesToRadians(20f),MathHelper.DegreesToRadians(170f));
                fov = MathHelper.Clamp(value, 40f, 170f);
            }
        }

        public Matrix4 GetViewMatrix(){
            return Matrix4.LookAt(cameraPosition, cameraPosition + front, up);
        }

        public Matrix4 GetProjectionMatrix(){
            return Matrix4.CreatePerspectiveFieldOfView(cameraFov, AspectRatio, 0.01f, 1000f);
        }

        private void UpdateVectors(){
            front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
            front.Y = MathF.Sin(pitch);
            front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);

            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public Vector3 getInput(){
            return cameraPosition;
        }

        public void ManageInput(FrameEventArgs e){

            speed = 1.5f;
            sensitivity = 0.2f;
            float time = (float)e.Time;

            if (keyboard.IsKeyDown(Keys.W))
            {
                cameraPosition += cameraFront * speed * time; // Forward
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                cameraPosition -= cameraFront * speed * time; // Backwards
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                cameraPosition -= cameraRight * speed * time; // Left
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                cameraPosition += cameraRight * speed * time; // Right
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                cameraPosition += cameraUp * speed * time; // Up
            }
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                cameraPosition -= cameraUp * speed * time; // Down
            }
            if (keyboard.IsKeyDown(Keys.B)){
                Console.WriteLine("{0}    {1}    {2}    {3}\n", cameraPosition, cameraFov, cameraFront, cameraYaw);
            }

            if(firstMove){
                lastMousePos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else{
                float deltaX = mouse.X - lastMousePos.X;
                float deltaY = mouse.Y - lastMousePos.Y;
                lastMousePos = new Vector2(mouse.X, mouse.Y);
                yaw += deltaX * sensitivity * time;
                pitch -= deltaY * sensitivity * time;
            }
            UpdateVectors();
        }

        public void OnMouseWheel(MouseWheelEventArgs e){
            cameraFov = MathHelper.RadiansToDegrees(cameraFov) - e.OffsetY * 2;
            //Console.WriteLine($"{e.Offset}");
            //cameraFov -= e.OffsetX;
        }
    }
}