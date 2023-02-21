using OpenTK.Mathematics;

using Primitives.Voxels;

namespace Level;

public class Chunk {

    public static Vector3 Size = new Vector3(16,64,16);
    Dictionary<Vector3, Block> BlocksInChunk = new Dictionary<Vector3, Block>();
    public readonly Vector3 Position;

    public Chunk(Vector3 pos = default){
        this.Position = pos;
        genArea();
    }

    public void AddBlockAt(Vector3 pos, Block vol){
        BlocksInChunk.Add(pos, vol);
        // Block v;
        // if (BlocksInChunk.TryGetValue(new Vector3(pos.X + 1, pos.Y, pos.Z), out v!)) v.UpdateSide(pos, true); vol.UpdateSide(v.Position, true);
        // if (BlocksInChunk.TryGetValue(new Vector3(pos.X, pos.Y + 1, pos.Z), out v!)) v.UpdateSide(pos, true); vol.UpdateSide(v.Position, true);
        // if (BlocksInChunk.TryGetValue(new Vector3(pos.X, pos.Y, pos.Z + 1), out v!)) v.UpdateSide(pos, true); vol.UpdateSide(v.Position, true);
        // if (BlocksInChunk.TryGetValue(new Vector3(pos.X - 1, pos.Y, pos.Z), out v!)) v.UpdateSide(pos, true); vol.UpdateSide(v.Position, true);
        // if (BlocksInChunk.TryGetValue(new Vector3(pos.X, pos.Y - 1, pos.Z), out v!)) v.UpdateSide(pos, true); vol.UpdateSide(v.Position, true);
        // if (BlocksInChunk.TryGetValue(new Vector3(pos.X, pos.Y, pos.Z - 1), out v!)) v.UpdateSide(pos, true); vol.UpdateSide(v.Position, true);
    }

    public Block GetBlockAt(Vector3 pos){
        return BlocksInChunk[pos];
    }

    public Dictionary<Vector3, Block>.Enumerator GetAll(){
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