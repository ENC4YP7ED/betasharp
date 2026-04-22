using BetaSharp.Client.Rendering.Core.OpenGL;

namespace BetaSharp.Client.Rendering.Core;

public class VertexBuffer<T> : IVertexBuffer<T> where T : unmanaged
{
    private uint id;
    private bool disposed;
    private int size;

    public unsafe VertexBuffer(Span<T> data, BufferUsage usage = BufferUsage.StaticDraw)
    {
        id = RenderDragon.Api.GenBuffer();
        RenderDragon.Api.BindBuffer(GLEnum.ArrayBuffer, id);
        RenderDragon.Api.BufferData<T>(GLEnum.ArrayBuffer, data, ToGLEnum(usage));
        size = data.Length * sizeof(T);
        VertexBufferStats.AllocatedBytes += size;
    }

    public void Bind()
    {
        if (disposed || id == 0)
        {
            throw new Exception("Attempted to bind invalid VertexBuffer");
        }

        RenderDragon.Api.BindBuffer(GLEnum.ArrayBuffer, id);
    }

    public unsafe void BufferData(Span<T> data, BufferUsage usage = BufferUsage.StaticDraw)
    {
        if (id == 0)
        {
            throw new Exception("Attempted to upload data to an invalid VertexBuffer");
        }
        else
        {
            RenderDragon.Api.BindBuffer(GLEnum.ArrayBuffer, id);
            RenderDragon.Api.BufferData(GLEnum.ArrayBuffer, (nuint)(data.Length * sizeof(T)), (void*)0, ToGLEnum(usage));
            RenderDragon.Api.BufferData<T>(GLEnum.ArrayBuffer, data, ToGLEnum(usage));

            VertexBufferStats.AllocatedBytes -= size;
            size = data.Length * sizeof(T);
            VertexBufferStats.AllocatedBytes += size;
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        GC.SuppressFinalize(this);

        if (id != 0)
        {
            RenderDragon.Api.DeleteBuffer(id);
            VertexBufferStats.AllocatedBytes -= size;
            size = 0;
            id = 0;
        }

        disposed = true;
    }

    private static GLEnum ToGLEnum(BufferUsage usage) => usage switch
    {
        BufferUsage.StreamDraw => GLEnum.StreamDraw,
        _ => GLEnum.StaticDraw
    };
}
