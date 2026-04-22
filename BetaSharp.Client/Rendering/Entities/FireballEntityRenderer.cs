using BetaSharp.Client.Rendering.Core;
using BetaSharp.Client.Rendering.Core.OpenGL;
using BetaSharp.Entities;
using BetaSharp.Items;

namespace BetaSharp.Client.Rendering.Entities;

public class FireballEntityRenderer : EntityRenderer
{

    public void render(EntityFireball var1, double x, double y, double z, float yaw, float tickDelta)
    {
        float var10 = 2.0F;
        RenderDragon.BeginBillboard((float)x, (float)y, (float)z, var10 / 1.0F, Dispatcher.PlayerViewY, Dispatcher.PlayerViewX);
        int var11 = Item.Snowball.getTextureId(0);
        loadTexture("/gui/items.png");
        Tessellator var12 = Tessellator.instance;
        float var13 = (var11 % 16 * 16 + 0) / 256.0F;
        float var14 = (var11 % 16 * 16 + 16) / 256.0F;
        float var15 = (var11 / 16 * 16 + 0) / 256.0F;
        float var16 = (var11 / 16 * 16 + 16) / 256.0F;
        float var17 = 1.0F;
        float var18 = 0.5F;
        float var19 = 0.25F;
        var12.startDrawingQuads();
        var12.setNormal(0.0F, 1.0F, 0.0F);
        var12.addVertexWithUV((double)(0.0F - var18), (double)(0.0F - var19), 0.0D, (double)var13, (double)var16);
        var12.addVertexWithUV((double)(var17 - var18), (double)(0.0F - var19), 0.0D, (double)var14, (double)var16);
        var12.addVertexWithUV((double)(var17 - var18), (double)(1.0F - var19), 0.0D, (double)var14, (double)var15);
        var12.addVertexWithUV((double)(0.0F - var18), (double)(1.0F - var19), 0.0D, (double)var13, (double)var15);
        var12.draw();
        RenderDragon.EndBillboard();
    }

    public override void Render(Entity target, double x, double y, double z, float yaw, float tickDelta)
    {
        render((EntityFireball)target, x, y, z, yaw, tickDelta);
    }
}
