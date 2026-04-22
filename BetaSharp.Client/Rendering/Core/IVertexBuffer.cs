namespace BetaSharp.Client.Rendering.Core;

public interface IVertexBuffer<T> : IDisposable where T : unmanaged
{
    void Bind();
    void BufferData(Span<T> data);
}
