using OpenTK.Mathematics;

namespace Primitives
{

public struct Quad{
    Vector3[] vertices;
    int[] indeces = new int[]{0, 2, 1, 1, 2, 3};

    public Quad(Vector3 bl, Vector3 br, Vector3 fl, Vector3 fr){
        vertices = new Vector3[]{
            bl,br,fl,fr
        };
    }

    public Vector3[] GetVertices(){
        return vertices;
    }

    public int[] GetIndeces(){
        return indeces;
    }

}
}