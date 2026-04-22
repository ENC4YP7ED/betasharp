using BetaSharp.Client.Rendering.Core.OpenGL;
using BetaSharp.Client.Rendering.Core.Textures;
using Microsoft.Extensions.Logging;
using Silk.NET.OpenGL;

namespace BetaSharp.Client.Rendering.Core;

public static class RenderDragon
{
    public static IGL Api { get; private set; } = null!;
    public static RenderBackendPreference PreferredBackend { get; private set; } = RenderBackendPreference.Auto;
    public static RenderBackendKind RequestedBackend { get; private set; } = RenderBackendKind.OpenGL;
    public static RenderBackendKind ActiveBackend { get; private set; } = RenderBackendKind.OpenGL;
    public static string? FallbackReason { get; private set; }

    public static bool IsInitialized => Api is not null;
    public static bool IsUsingFallback => FallbackReason is not null;

    public static RenderBackendKind SelectBackend(RenderBackendPreference preference, ILogger? logger = null)
    {
        PreferredBackend = preference;
        RequestedBackend = ResolveRequestedBackend(preference);
        FallbackReason = null;
        ActiveBackend = RequestedBackend;

        if (RequestedBackend == RenderBackendKind.Vulkan)
        {
            FallbackReason = "The current renderer still depends on fixed-function OpenGL semantics, so Vulkan is not available yet. Falling back to OpenGL.";
            logger?.LogWarning("RenderDragon backend fallback: {Reason}", FallbackReason);
            ActiveBackend = RenderBackendKind.OpenGL;
        }

        logger?.LogInformation(
            "RenderDragon backend selected. Preferred={PreferredBackend}, Requested={RequestedBackend}, Active={ActiveBackend}",
            PreferredBackend,
            RequestedBackend,
            ActiveBackend);

        return ActiveBackend;
    }

    public static void BindOpenGL(GL silkGl, ILogger? logger = null)
    {
        if (ActiveBackend != RenderBackendKind.OpenGL)
        {
            throw new NotSupportedException($"RenderDragon cannot bind an OpenGL API while the active backend is {ActiveBackend}.");
        }

        Api = new EmulatedGL(silkGl);

        logger?.LogInformation(
            "RenderDragon API bound. Preferred={PreferredBackend}, Requested={RequestedBackend}, Active={ActiveBackend}",
            PreferredBackend,
            RequestedBackend,
            ActiveBackend);
    }

    public static ITexture CreateTexture(string source) => ActiveBackend switch
    {
        RenderBackendKind.OpenGL => new GLTexture(source),
        RenderBackendKind.Vulkan => throw CreateUnsupportedBackendException("texture creation"),
        _ => throw CreateUnsupportedBackendException("texture creation"),
    };

    public static IFramebuffer CreateFramebuffer(int width, int height) => ActiveBackend switch
    {
        RenderBackendKind.OpenGL => new Framebuffer(width, height),
        RenderBackendKind.Vulkan => throw CreateUnsupportedBackendException("framebuffer creation"),
        _ => throw CreateUnsupportedBackendException("framebuffer creation"),
    };

    public static IShader CreateShader(string vertexShaderSource, string fragmentShaderSource) => ActiveBackend switch
    {
        RenderBackendKind.OpenGL => new Shader(vertexShaderSource, fragmentShaderSource),
        RenderBackendKind.Vulkan => throw CreateUnsupportedBackendException("shader creation"),
        _ => throw CreateUnsupportedBackendException("shader creation"),
    };

    public static IVertexArray CreateVertexArray() => ActiveBackend switch
    {
        RenderBackendKind.OpenGL => new VertexArray(),
        RenderBackendKind.Vulkan => throw CreateUnsupportedBackendException("vertex array creation"),
        _ => throw CreateUnsupportedBackendException("vertex array creation"),
    };

    public static IVertexBuffer<T> CreateVertexBuffer<T>(Span<T> data) where T : unmanaged => ActiveBackend switch
    {
        RenderBackendKind.OpenGL => new VertexBuffer<T>(data),
        RenderBackendKind.Vulkan => throw CreateUnsupportedBackendException("vertex buffer creation"),
        _ => throw CreateUnsupportedBackendException("vertex buffer creation"),
    };

    public static void UnbindFramebuffer()
    {
        Api.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public static void UnbindVertexArray()
    {
        Api.BindVertexArray(0);
    }

    private static RenderBackendKind ResolveRequestedBackend(RenderBackendPreference preference) => preference switch
    {
        RenderBackendPreference.PreferVulkan => RenderBackendKind.Vulkan,
        RenderBackendPreference.Auto => RenderBackendKind.OpenGL,
        _ => RenderBackendKind.OpenGL,
    };

    private static NotSupportedException CreateUnsupportedBackendException(string operation)
    {
        return new NotSupportedException($"RenderDragon does not yet support {operation} for backend {ActiveBackend}.");
    }
}
