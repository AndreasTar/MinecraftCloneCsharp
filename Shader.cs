using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

public class Shader
{
    public readonly int Handle;

    private bool disposedValue = false;

    private readonly Dictionary<string,int> uniformlocs;

    public Shader(string vertexPath, string fragmentPath)
    {

        string VertexShaderSource = File.ReadAllText(vertexPath);
        string FragmentShaderSource = File.ReadAllText(fragmentPath);

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader,VertexShaderSource);

        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader,FragmentShaderSource);

        GL.CompileShader(VertexShader);
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int successv);
        if (successv == 0){
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(FragmentShader);
        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int successf);
        if (successf == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);
        GL.GetProgram(Handle,GetProgramParameterName.LinkStatus, out int successh);
        if(successh == 0){
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }

        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);

        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
        uniformlocs = new Dictionary<string, int>();
        for (int i = 0; i < numberOfUniforms; i++)
        {
            string key = GL.GetActiveUniform(Handle, i, out _, out _);
            int location = GL.GetUniformLocation(Handle, key);
            uniformlocs.Add(key,location);
        }
    }

    public void Use(){
        GL.UseProgram(Handle);
    }

    protected virtual void Dispose(bool disposing){
        if (!disposedValue){
            GL.DeleteProgram(Handle);
            disposedValue = true;
        }
    }

    public int GetAttribLocation(string attribName){
        return GL.GetAttribLocation(Handle, attribName);
    }

    public int GetUniformLocation(string unifromName){
        return GL.GetUniformLocation(Handle, unifromName);
    }

    // Destructor, used by garbage collector
    ~Shader(){
        GL.DeleteProgram(Handle);
    }

    public void Dispose(){
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void SetMatrix4(string name, Matrix4 data){
        GL.UseProgram(Handle);
        GL.UniformMatrix4(uniformlocs[name], true, ref data);
    }
}