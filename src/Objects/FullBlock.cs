using OpenTK.Mathematics;

using Primitives.Voxels;

public class FullBlock : Block{
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
}