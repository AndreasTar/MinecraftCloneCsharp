using OpenTK.Mathematics;

public class Block : Volume{
    public Block(){
        VertCount = 8;
        IndexCount = 36;
        ColorDataCount = 8;
    }

    public override void CalculateModelMatrix()
    {
        ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) *
             Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) *
             Matrix4.CreateTranslation(Position);
    }

    public override Vector3[] GetColorData()
    {
        return new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 0f, 1f)
        };
    }

    public override int[] GetIndices(int offset = 0)
    {
        int[] inds = new int[]{
                0, 6, 2, // left face
                0, 4, 6,
                0, 5, 4, // back face
                0, 1, 5,
                5, 3, 7, // right face
                1, 3, 5,
                2, 6, 3, // front face
                3, 6, 7,
                4, 5, 6, // top face
                5, 7, 6,
                0, 3, 1, // bottom face
                0, 2, 3
        };

        if (offset !=0){
            for (int i = 0; i < inds.Length; i++)
            {
                inds[i] += offset;
            }
        }

        return inds;
    }

    public override Vector3[] GetVerts()
    {
        return new Vector3[] {
            // bot
            new Vector3(0f,0f,0f), // back left
            new Vector3(1f,0f,0f), // back right
            new Vector3(0f,0f,1f), // front left
            new Vector3(1f,0f,1f), // front right
            // top
            new Vector3(0f,1f,0f), // back left
            new Vector3(1f,1f,0f), // back right
            new Vector3(0f,1f,1f), // front left
            new Vector3(1f,1f,1f)  // front right
        };
    }
}