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
        float pitch = 0f;
        float yaw = 270f;
        float fov = 80f;

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

        public CameraControl(Vector3 pos, float ar, KeyboardState keyb, MouseState mous){
            cameraPosition = pos;
            AspectRatio = ar;
            keyboard = keyb;
            mouse = mous;
        }

        public float cameraPitch // degrees
        {
            get => pitch;
            set {
                pitch = MathHelper.Clamp(value, -89, 89); // clamp the value itself
                UpdateVectors();
            }
        }
        public float cameraYaw // degrees
        {
            get => yaw;
            set
            {
                yaw = value;
                if(yaw > 360) yaw -= 360;
                if(yaw < 0) yaw += 360;
                UpdateVectors();
            }
        }
        public float cameraFov // degrees
        {
            get => fov;
            set
            {
                fov = MathHelper.Clamp(value, 40f, 140f);
            }
        }

        public Matrix4 GetViewMatrix(){
            return Matrix4.LookAt(cameraPosition, cameraPosition + front, up);
        }

        public Matrix4 GetProjectionMatrix(){
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(cameraFov), AspectRatio, 0.01f, 1000f);
        }

        private void UpdateVectors(){
            float tempp = MathHelper.DegreesToRadians(cameraPitch);
            float tempy = MathHelper.DegreesToRadians(cameraYaw);
            
            front.X = MathF.Cos(tempp) * MathF.Cos(tempy);
            front.Y = MathF.Sin(tempp);
            front.Z = MathF.Cos(tempp) * MathF.Sin(tempy);

            front = Vector3.Normalize(front);
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public void ManageInput(FrameEventArgs e){

            speed = 8.0f;
            sensitivity = .2f;
            float deltaTime = (float)e.Time;

            //Vector3 direction = Vector3.Zero;
            //Vector3 movement = Vector3.Zero;

            if (keyboard.IsKeyDown(Keys.W))
            {
                cameraPosition += cameraFront * speed * deltaTime; // Forward
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                cameraPosition -= cameraFront * speed * deltaTime; // Backwards
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                cameraPosition -= cameraRight * speed * deltaTime; // Left
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                cameraPosition += cameraRight * speed * deltaTime; // Right
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                cameraPosition += cameraUp * speed * deltaTime; // Up
            }
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                cameraPosition -= cameraUp * speed * deltaTime; // Down
            }

            // direction.Normalize();
            // if (!(Double.IsNaN(direction.X) && Double.IsNaN(direction.Y) && Double.IsNaN(direction.Z))){
            //     float tempspeed = speed / deltaTime;
            //     movement = tempspeed * direction;
            // }

            if (keyboard.IsKeyDown(Keys.B)){
                Console.WriteLine("{0}    {1}    {2}    {3}    {4}    {5}\n", cameraPosition, cameraFov, cameraFront, cameraYaw, cameraPitch, lastMousePos);
            }

            if (firstMove){
                lastMousePos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else{
                float deltaX = mouse.X - lastMousePos.X;
                float deltaY = mouse.Y - lastMousePos.Y;
                lastMousePos = new Vector2(mouse.X, mouse.Y);
                cameraYaw += deltaX * sensitivity;
                cameraPitch -= deltaY * sensitivity;
            }
            UpdateVectors();
        }

        public void OnMouseWheel(MouseWheelEventArgs e){
            cameraFov -= e.OffsetY * 2;
        }
    }
}