using BetaSharp.Client.Rendering.Core.OpenGL;

namespace BetaSharp.Client.Rendering.Core;

public sealed class LegacyMesh : ILegacyMesh
{
    private readonly IVertexArray _vertexArray;
    private readonly IVertexBuffer<Vertex> _vertexBuffer;
    private readonly int _vertexCount;
    private bool _disposed;

    public unsafe LegacyMesh(Span<Vertex> vertices, LegacyMeshLayout layout)
    {
        IGL gl = RenderDragon.Api;
        _vertexArray = RenderDragon.CreateVertexArray();
        _vertexBuffer = RenderDragon.CreateVertexBuffer(vertices);
        _vertexCount = vertices.Length;

        _vertexArray.Bind();
        _vertexBuffer.Bind();

        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 32, (void*)0);

        if (layout.HasColor)
        {
            gl.EnableVertexAttribArray(1);
            gl.VertexAttribPointer(1, 4, GLEnum.UnsignedByte, true, 32, (void*)20);
        }

        if (layout.HasTextureCoordinates)
        {
            gl.EnableVertexAttribArray(2);
            gl.VertexAttribPointer(2, 2, GLEnum.Float, false, 32, (void*)12);
        }

        if (layout.HasNormals)
        {
            gl.EnableVertexAttribArray(3);
            gl.VertexAttribPointer(3, 3, GLEnum.Byte, true, 32, (void*)24);
        }

        RenderDragon.UnbindVertexArray();
    }

    public void Draw()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(LegacyMesh));
        }

        _vertexArray.Bind();
        RenderDragon.Api.DrawArrays(GLEnum.Triangles, 0, (uint)_vertexCount);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _vertexArray.Dispose();
        _vertexBuffer.Dispose();
        _disposed = true;
    }
}
