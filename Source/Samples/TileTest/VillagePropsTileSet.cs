using FlexoGraphics.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TileTest
{
    public class VillagePropsTileSet : ComplexTileSet
    {
        public VillagePropsTileSet(ContentManager content)
            : base(content, "TX Village Props")
        {
            RegisterTileRegions();
        }

        public Point BigBoxTile => new Point(0, 0);

        private void RegisterTileRegions()
        {
            // register logical col, row indexes to tileset rect areas.
            RegisterTileRegion(BigBoxTile, new Rectangle(3, 4, 46, 46));
        }
    }
}
