using Microsoft.Xna.Framework;

namespace ExoGame2D.Renderers
{
    public interface IBoundingBox
    {
        Rectangle BoundingBox { get; }
        bool RenderBoundingBox { get; set; }
    }
}
