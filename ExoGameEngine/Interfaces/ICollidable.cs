using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;

namespace ExoGame2D.Interfaces
{
    public interface ICollidable : IBoundingBox, INamedObject
    {
        // for pixel perfect collision
        bool GetPixelData(Rectangle intersection, out Color[] data);
    }
}
