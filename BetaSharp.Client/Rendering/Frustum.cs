using Silk.NET.Maths;
using BetaSharp.Util.Maths;

namespace BetaSharp.Client.Rendering;

public class Frustum : FrustumData
{

    private static readonly Frustum _instance = new();

    public static FrustumData Instance(Matrix4X4<float> modelViewMatrix, Matrix4X4<float> projectionMatrix)
    {
        _instance.Initialize(modelViewMatrix, projectionMatrix);
        return _instance;
    }


    private void Normalize(int side)
    {
        int offset = side * 4;
        float x = Frustum[offset];
        float y = Frustum[offset + 1];
        float z = Frustum[offset + 2];

        float length = MathHelper.Sqrt(x * x + y * y + z * z);

        Frustum[offset] /= length;
        Frustum[offset + 1] /= length;
        Frustum[offset + 2] /= length;
        Frustum[offset + 3] /= length;
    }

    private void Initialize(Matrix4X4<float> modelViewMatrix, Matrix4X4<float> projectionMatrix)
    {
        CopyMatrix(projectionMatrix, ProjectionMatrix);
        CopyMatrix(modelViewMatrix, ModelviewMatrix);

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ClippingMatrix[i * 4 + j] =
                    ModelviewMatrix[i * 4 + 0] * ProjectionMatrix[0 + j] +
                    ModelviewMatrix[i * 4 + 1] * ProjectionMatrix[4 + j] +
                    ModelviewMatrix[i * 4 + 2] * ProjectionMatrix[8 + j] +
                    ModelviewMatrix[i * 4 + 3] * ProjectionMatrix[12 + j];
            }
        }
        // Right Plane
        SetPlane(0, ClippingMatrix[3] - ClippingMatrix[0], ClippingMatrix[7] - ClippingMatrix[4], ClippingMatrix[11] - ClippingMatrix[8], ClippingMatrix[15] - ClippingMatrix[12]);
        // Left Plane
        SetPlane(1, ClippingMatrix[3] + ClippingMatrix[0], ClippingMatrix[7] + ClippingMatrix[4], ClippingMatrix[11] + ClippingMatrix[8], ClippingMatrix[15] + ClippingMatrix[12]);
        // Bottom Plane
        SetPlane(2, ClippingMatrix[3] + ClippingMatrix[1], ClippingMatrix[7] + ClippingMatrix[5], ClippingMatrix[11] + ClippingMatrix[9], ClippingMatrix[15] + ClippingMatrix[13]);
        // Top Plane
        SetPlane(3, ClippingMatrix[3] - ClippingMatrix[1], ClippingMatrix[7] - ClippingMatrix[5], ClippingMatrix[11] - ClippingMatrix[9], ClippingMatrix[15] - ClippingMatrix[13]);
        // Far Plane
        SetPlane(4, ClippingMatrix[3] - ClippingMatrix[2], ClippingMatrix[7] - ClippingMatrix[6], ClippingMatrix[11] - ClippingMatrix[10], ClippingMatrix[15] - ClippingMatrix[14]);
        // Near Plane
        SetPlane(5, ClippingMatrix[3] + ClippingMatrix[2], ClippingMatrix[7] + ClippingMatrix[6], ClippingMatrix[11] + ClippingMatrix[10], ClippingMatrix[15] + ClippingMatrix[14]);
    }

    private static void CopyMatrix(Matrix4X4<float> source, float[] destination)
    {
        destination[0] = source.M11;
        destination[1] = source.M12;
        destination[2] = source.M13;
        destination[3] = source.M14;
        destination[4] = source.M21;
        destination[5] = source.M22;
        destination[6] = source.M23;
        destination[7] = source.M24;
        destination[8] = source.M31;
        destination[9] = source.M32;
        destination[10] = source.M33;
        destination[11] = source.M34;
        destination[12] = source.M41;
        destination[13] = source.M42;
        destination[14] = source.M43;
        destination[15] = source.M44;
    }

    private void SetPlane(int side, float a, float b, float c, float d)
    {
        int offset = side * 4;
        Frustum[offset] = a;
        Frustum[offset + 1] = b;
        Frustum[offset + 2] = c;
        Frustum[offset + 3] = d;
        Normalize(side);
    }
}
