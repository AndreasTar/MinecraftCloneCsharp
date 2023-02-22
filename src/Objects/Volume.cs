using OpenTK.Mathematics;

namespace Primitives;
public abstract class Volume
 
    {   
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
 
        // public int VertCount;
        // public int IndexCount;
        // public int ColorDataCount;
        public Quad[]? quads;
        public Matrix4 ModelMatrix = Matrix4.Identity;
 
        public abstract Vector3[] GetVerts();
        public abstract int[] GetIndices(int offset = 0);
        public abstract Vector3[] GetColorData();
        public abstract void CalculateModelMatrix();
    }