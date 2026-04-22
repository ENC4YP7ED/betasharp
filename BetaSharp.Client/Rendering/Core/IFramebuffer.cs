namespace BetaSharp.Client.Rendering.Core;

public interface IFramebuffer : IDisposable
{
    uint TextureId { get; }
    int Width { get; }
    int Height { get; }

    void Bind();
    void Resize(int width, int height);
}
