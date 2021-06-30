using ExoGame2D.Tiles;
using Microsoft.Xna.Framework;

namespace TileTest
{
    public class VillagePropsTileSet : ComplexTileSet
    {
        public VillagePropsTileSet()
            : base("TX Village Props")
        {
            RegisterTileRegions();
        }

        public Point BigBoxTile => new Point(0, 0);

        private void RegisterTileRegions()
        {
            RegisterTileRegion(0, 0, new Rectangle(3, 4, 46, 46));
        }
    }
}
