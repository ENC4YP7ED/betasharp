using Silk.NET.OpenGL;

namespace BetaSharp.Client.Rendering.Core.Textures;

public interface ITexture : IDisposable
{
    uint Id { get; }
    string Source { get; }
    int Width { get; }
    int Height { get; }

    void Bind();
    void SetFilter(TextureMinFilter min, TextureMagFilter mag);
    void SetWrap(TextureWrapMode s, TextureWrapMode t);
    void SetMaxLevel(int level);
    unsafe void Upload(int width, int height, byte* ptr, int level = 0, PixelFormat format = PixelFormat.Rgba, InternalFormat internalFormat = InternalFormat.Rgba);
    unsafe void UploadSubImage(int x, int y, int width, int height, byte* ptr, int level = 0, PixelFormat format = PixelFormat.Rgba);
    void SetAnisotropicFilter(float level);
}
