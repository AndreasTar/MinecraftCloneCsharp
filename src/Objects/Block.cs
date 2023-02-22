using OpenTK.Mathematics;

namespace Primitives.Voxels;
public class Block : Volume{

    /*
        order of quads :

        south   0 = front
        north   1 = back
        east    2 = right
        west    3 = left
        up      4
        down    5
    */

    /// <summary>
    /// Is this face of the Block full, meaning does it cover the face of the block next to it fully?
    /// </summary>
    public bool
        top = false,
        bot = false,
        north = false,
        south = false,
        east = false,
        west = false;


    public Block(){
        quads = new Quad[6];
        setDefaults();
    }

    public Block(Vector3 pos){
        quads = new Quad[6];
        Position = pos;
        setDefaults();
    }

    public Block(int x, int y, int z){
        quads = new Quad[6];
        Position = new Vector3(x,y,z);
        setDefaults();
    }

    public virtual void setDefaults(){
        // south front
        quads![0] = new Quad(new Vector3(0f, 0f, 1f), // down left
                             new Vector3(1f, 0f, 1f), // down right
                             new Vector3(0f, 1f, 1f), // up left
                             new Vector3(1f, 1f, 1f)  // up right
        );
        // north back
        quads![1] = new Quad(new Vector3(0f, 0f, 0f), // down left
                             new Vector3(1f, 0f, 0f), // down right
                             new Vector3(0f, 1f, 0f), // up left
                             new Vector3(1f, 1f, 0f)  // up right
        );
        // east right
        quads![2] = new Quad(new Vector3(1f, 0f, 1f), // front down
                             new Vector3(1f, 0f, 0f), // back down
                             new Vector3(1f, 1f, 1f), // front up
                             new Vector3(1f, 1f, 0f)  // back up
        );
        // west left
        quads![3] = new Quad(new Vector3(0f, 0f, 1f), // front down
                             new Vector3(0f, 0f, 0f), // back down
                             new Vector3(0f, 1f, 1f), // front up
                             new Vector3(0f, 1f, 0f)  // back up
        );
        // up
        quads![4] = new Quad(new Vector3(0f, 1f, 0f), // back left
                             new Vector3(1f, 1f, 0f), // back right
                             new Vector3(0f, 1f, 1f), // front left
                             new Vector3(1f, 1f, 1f)  // front right
        );
        // down
        quads![5] = new Quad(new Vector3(0f, 0f, 0f), // back left
                             new Vector3(1f, 0f, 0f), // back right
                             new Vector3(0f, 0f, 1f), // front left
                             new Vector3(1f, 0f, 1f)  // front right
        );
    }

    public override void CalculateModelMatrix()
    {
        ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateRotationX(Rotation.X) *
             Matrix4.CreateRotationY(Rotation.Y) * Matrix4.CreateRotationZ(Rotation.Z) *
             Matrix4.CreateTranslation(Position);
    }

    public override Vector3[] GetColorData()
    {
        Vector3[] cd;
        for (int i = 0; i < 6; i++)
        {
            cd.Append(quads![i].GetVertexColors());
        }
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
