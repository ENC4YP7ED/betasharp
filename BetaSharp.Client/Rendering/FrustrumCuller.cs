using BetaSharp.Util.Maths;

namespace BetaSharp.Client.Rendering;

public class FrustrumCuller : ICuller
{

    private readonly FrustumData _frustum;
    private double _x;
    private double _y;
    private double _z;

    public FrustrumCuller(FrustumData frustum)
    {
        _frustum = frustum;
    }

    public void SetPosition(double x, double y, double z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public bool IsBoxInFrustum(Box box)
    {
        return _frustum.IsBoxInFrustum(box.Offset(-_x, -_y, -_z));
    }

    public bool IsBoundingBoxInFrustum(Box aabb)
    {
        return IsBoxInFrustum(aabb);
    }
}
