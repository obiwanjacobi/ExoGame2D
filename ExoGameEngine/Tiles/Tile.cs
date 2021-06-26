using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoGame2D.Tiles
{
    public class Tile : IRenderNode
    {

        private readonly TileSet _tileSet;
        private readonly Rectangle _tileFromSetCoord;

        internal Tile(TileSet tileSet, Rectangle tileFromSetCoord)
        {
            _tileSet = tileSet;
            _tileFromSetCoord = tileFromSetCoord;
        }

        public Vector2 Location { get; set; }

        public void Draw(GameTime gameTime)
        {
            var effect = SpriteEffects.None;
            var tint = Color.White;
            _tileSet.DrawTile(_tileFromSetCoord, Location, effect, tint);
        }

        public void Update(GameTime gameTime)
        {
            // Tiles dont move
        }
    }
}
