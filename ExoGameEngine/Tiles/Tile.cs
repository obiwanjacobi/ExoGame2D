using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoGame2D.Tiles
{
    public class Tile : IRenderNode
    {
        private readonly TileSet _tileSet;
        private readonly Rectangle _tileOrigin;

        internal Tile(TileSet tileSet, Rectangle tileOrigin)
        {
            _tileSet = tileSet;
            _tileOrigin = tileOrigin;
        }

        public Point Size => _tileOrigin.Size;

        public Vector2 Location { get; set; }

        public void Draw(DrawContext context, GameTime gameTime)
        {
            var effect = SpriteEffects.None;
            var tint = Color.White;
            _tileSet.DrawTile(context, _tileOrigin, Location, effect, tint);
        }

        public void Update(GameTime gameTime)
        {
            // Tiles dont move
        }
    }
}
