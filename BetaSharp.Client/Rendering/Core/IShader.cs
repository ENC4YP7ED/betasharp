using Silk.NET.Maths;

namespace BetaSharp.Client.Rendering.Core;

public interface IShader : IDisposable
{
    void Bind();
    void SetUniform3(string name, Vector3D<float> vec);
    void SetUniform1(string name, float value);
    void SetUniform1(string name, int value);
    void SetUniform2(string name, float x, float y);
    void SetUniformMatrix4(string name, Matrix4X4<float> matrix);
    void SetUniformMatrix4(string name, float[] matrix);
    void SetUniform4(string name, Vector4D<float> vec);
}
