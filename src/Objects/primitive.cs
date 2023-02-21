using OpenTK.Mathematics;

namespace Primitives
{
struct VertexColor {
    Vector3 Position;
    Vector4 Color;
    
    VertexColor(Vector3 pos, Vector4 col){
        this.Position = pos;
        this.Color = col;
    }
}

struct VertexTexture {
    Vector3 Position;
    Vector2 TextureCoord;

    VertexTexture(Vector3 pos, Vector2 tc){
        this.Position = pos;
        this.TextureCoord = tc;
    }
}

struct VertexColorTexture
{
    Vector3 Position;
    Vector4 Color;
    Vector2 TextureCoord;

    VertexColorTexture(Vector3 pos, Vector4 col, Vector2 tc){
        this.Position = pos;
        this.Color = col;
        this.TextureCoord = tc;
    }
}
}