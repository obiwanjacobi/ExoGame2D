using FlexoGraphics.Renderers;
using Microsoft.Xna.Framework;

namespace FlexoGraphics.Interfaces
{
    public interface ICollidable : IBoundingBox, INamedObject
    {
        // for pixel perfect collision
        bool GetPixelData(Rectangle intersection, out Color[] data);
    }
}
