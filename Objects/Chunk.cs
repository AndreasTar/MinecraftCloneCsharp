using OpenTK.Mathematics;

public class Chunk {

    public static Vector3 Size = new Vector3(16,64,16);
    Dictionary<Vector3, Volume> BlocksInChunk = new Dictionary<Vector3, Volume>();
    public readonly Vector3 Position;

    public Chunk(Vector3 pos = default){
        this.Position = pos;
        genArea();
    }

    public void AddBlockAt(Vector3 pos, Volume vol){
        BlocksInChunk.Add(pos, vol);
    }

    public Volume GetBlockAt(Vector3 pos){
        return BlocksInChunk[pos];
    }

    public Dictionary<Vector3, Volume>.Enumerator GetAll(){
        return BlocksInChunk.GetEnumerator();
    }

    void genArea(){
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                AddBlockAt(new Vector3(x,0,z), new Block(x,0,z));
            }
        }
    }
}