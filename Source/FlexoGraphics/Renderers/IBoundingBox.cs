using Microsoft.Xna.Framework;

namespace FlexoGraphics.Renderers
{
    public interface IBoundingBox
    {
        Rectangle BoundingBox { get; }
        bool RenderBoundingBox { get; set; }
    }
}
