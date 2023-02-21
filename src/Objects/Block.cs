using OpenTK.Mathematics;

namespace Primitives.Voxels;
public class Block : Volume{

    /// <summary>
    /// Is this face of the Block covered by some other opaque Block?
    /// </summary>
    bool
        top = false,
        bot = false,
        north = false,
        south = false,
        east = false,
        west = false;

    protected Vector3[] defaultVertices = new Vector3[] { // looking at north, back bot left is 000 front top right is 111
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
    Vector3[]? updatedVertices;

    protected int[] defaultIndexes = new int[] {
        0, 2, 6, // left face
        0, 6, 4,
        0, 4, 5, // back face
        0, 5, 1,
        5, 7, 3, // right face
        1, 5, 3,
        2, 3, 6, // front face
        3, 7, 6,
        4, 6, 5, // top face
        5, 6, 7,
        0, 1, 3, // bottom face
        0, 3, 2
    };
    int[]? updatedIndexes;

    protected Vector3[] defaultColors = new Vector3[] {
        new Vector3(1f, 0f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(0f, 1f, 0f),
        new Vector3(1f, 0f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(0f, 1f, 0f),
        new Vector3(1f, 0f, 0f),
        new Vector3(0f, 0f, 1f)
    };
    Vector3[]? updatedColors;

    public Block(){
        VertCount = 8;
        IndexCount = 36;
        ColorDataCount = 8;
        setDefaults();
    }

    public Block(Vector3 pos){
        VertCount = 8;
        IndexCount = 36;
        ColorDataCount = 8;
        Position = pos;
        setDefaults();
    }

    public Block(int x, int y, int z){
        VertCount = 8;
        IndexCount = 36;
        ColorDataCount = 8;
        Position = new Vector3(x,y,z);
        setDefaults();
    }

    void setDefaults(){
        updatedVertices = defaultVertices;
        updatedIndexes = defaultIndexes;
        updatedColors = defaultColors;
    }

    public override void CalculateModelMatrix()
    {
        ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) *
             Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) *
             Matrix4.CreateTranslation(Position);
    }

    public override Vector3[] GetColorData()
    {
        return updatedColors!;
    }

    public override int[] GetIndices(int offset = 0)
    {
        updatedIndexes = defaultIndexes;
        
        if (offset !=0){
            for (int i = 0; i < updatedIndexes!.Length; i++)
            {
                updatedIndexes[i] += offset;
            }
        }

        return updatedIndexes!;
    }

    public override Vector3[] GetVerts()
    {
        return updatedVertices!;
    }

    public virtual void UpdateSide(Vector3 other, bool added){
        if(other.X > Position.X) {
            // east from this
            east = added;
        }
        else if(other.Y > Position.Y) {
            // up from this
            top = added;
        }
        else if(other.Z > Position.Z) {
            // south from this
            south = added;
        }
        else if(other.X < Position.X) {
            // west from this
            west = added;
        }
        else if(other.Y < Position.Y) {
            // down from this
            bot = added;
        }
        else if(other.Z < Position.Z) {
            // north from this
            north = added;
        }
    }
}
